using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using LibTads.Autores.Dto;
using LibTads.Domain;
using LibTads.Emprestimos.Dto;
using LibTads.Livros.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibTads.Emprestimos
{
    public class EmprestimoAppService : LibTadsAppServiceBase
    {
        private readonly IRepository<Emprestimo, int> Repository;
        private readonly IRepository<Livro, int> _livroRepository;
        private readonly IAbpSession _abpSession;

        public EmprestimoAppService(IAbpSession abpSession, IRepository<Livro, int> livroRepository, IRepository<Emprestimo, int> _repository)
        {
            Repository = _repository;
            _livroRepository = livroRepository;
            _abpSession = abpSession;
        }

        public async Task<EmprestimoDto> CreateAsync(CreateEmprestimoDto emprestimoDto)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Erro: Faça o login novamente!");
            }
            emprestimoDto.UserId = _abpSession.GetUserId();
            var haveEmprestuimo = await Repository.FirstOrDefaultAsync(x => x.LivroId.Equals(emprestimoDto.LivroId) && x.UserId.Equals(emprestimoDto.UserId) && x.DataDevolucao == null);
            if (haveEmprestuimo != null)
            {
                throw new UserFriendlyException("Emprestimo em andamento!");
            }

            var livro = await _livroRepository.FirstOrDefaultAsync(x => x.Id.Equals(emprestimoDto.LivroId));
            livro.QuantidadeDisponivel -= 1;
            await _livroRepository.UpdateAsync(livro);

            var emprestimo = ObjectMapper.Map<Emprestimo>(emprestimoDto);
            emprestimo.DataEmprestimo = DateTime.Now;
            emprestimo.CreationTime = DateTime.Now;
            await Repository.InsertAsync(emprestimo);
            return ObjectMapper.Map<EmprestimoDto>(emprestimo);
        }

        public async Task<PagedResultDto<EmprestimoDto>> GetAllEmprestimos(PagedEmprestimoResultRequestDto input, int pageNumber, int pageSize)
        {
            if (input.Keyword == null) input.Keyword = "";

            var emprestimos = Repository.GetAll()
                .Include(x => x.Usuario)
                .Include(x => x.Livro)
                .Where(x => x.Livro.Titulo.Contains(input.Keyword) || x.Usuario.Name.Contains(input.Keyword))
                .OrderByDescending(x => x.DataEmprestimo);

            var items = emprestimos.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            var emprestimosDto = ObjectMapper.Map<List<EmprestimoDto>>(items);
            foreach (var item in emprestimosDto)
            {
                var emprestimoMap = items.FirstOrDefault(x => x.Id == item.Id);
                item.TituloLivro = emprestimoMap.Livro.Titulo;
                item.NomeUsuario = emprestimoMap.Usuario.Name;
            }

            var totalCount = emprestimos.Count();
            return new PagedResultDto<EmprestimoDto>(totalCount, emprestimosDto);
        }

        public async Task<PagedResultDto<EmprestimoDto>> GetAllEmprestimosByUser(PagedEmprestimoResultRequestDto input, int pageNumber, int pageSize)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Erro: Faça o login novamente!");
            }
            if (input.Keyword == null) input.Keyword = "";
            var emprestimos = Repository.GetAll()
                .Include(x => x.Usuario)
                .Include(x => x.Livro)
                .Where(x => x.UserId.Equals(_abpSession.GetUserId()) && (x.Livro.Titulo.Contains(input.Keyword) || x.Usuario.Name.Contains(input.Keyword)))
                .OrderByDescending(x => x.DataEmprestimo);

            var items = emprestimos.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            var emprestimosDto = ObjectMapper.Map<List<EmprestimoDto>>(items);
            foreach (var item in emprestimosDto)
            {
                var emprestimoMap = emprestimos.FirstOrDefault(x => x.Id == item.Id);
                item.TituloLivro = emprestimoMap.Livro.Titulo;
                item.NomeUsuario = emprestimoMap.Usuario.Name;
            }

            var totalCount = emprestimos.Count();
            return new PagedResultDto<EmprestimoDto>(totalCount, emprestimosDto);
        }
        public void DevolverLivro(int idEmprestimo)
        {
            var emprestimo = Repository.FirstOrDefault(x => x.Id.Equals(idEmprestimo));
            if (emprestimo == null)
            {
                throw new UserFriendlyException("Empréstimo não encontrado!");
            }

            var livro = _livroRepository.FirstOrDefault(x => x.Id.Equals(emprestimo.LivroId));
            if (livro == null)
            {
                throw new UserFriendlyException("Livro não encontrado!");
            }

            livro.QuantidadeDisponivel += 1;
            _livroRepository.UpdateAsync(livro);

            emprestimo.DataDevolucao = DateTime.Now;
            Repository.Update(emprestimo);
        }

        public void RenovarEmprestimo(int idEmprestimo)
        {
            var emprestimo = Repository.FirstOrDefault(x => x.Id.Equals(idEmprestimo));
            if (emprestimo == null)
            {
                throw new UserFriendlyException("Empréstimo não encontrado!");
            }

            emprestimo.DataDevolucao = DateTime.Now;
            Repository.Update(emprestimo);

            var newEmprestimo = new Emprestimo()
            {
                LivroId = emprestimo.LivroId,
                UserId = emprestimo.UserId,
                DataEmprestimo = DateTime.Now,
                CreationTime = DateTime.Now,
            };

            Repository.Insert(newEmprestimo);
        }
    }
}
