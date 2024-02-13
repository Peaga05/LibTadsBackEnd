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
    public class EmprestimoAppService : AsyncCrudAppService<Emprestimo, EmprestimoDto, int, PagedEmprestimoResultRequestDto, CreateEmprestimoDto, UpdateEmprestimoDto>
    {
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Livro, int> _livroRepository;
        public EmprestimoAppService(IRepository<Emprestimo, int> repository, IAbpSession abpSession, IRepository<Livro, int> livroRepository) : base(repository)
        {
            _abpSession = abpSession;
            _livroRepository = livroRepository;
        }

        public override async Task<EmprestimoDto> CreateAsync(CreateEmprestimoDto emprestimoDto)
        {
            CheckCreatePermission();
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

            var livro = _livroRepository.FirstOrDefault(x => x.Equals(emprestimoDto.LivroId));
            livro.Quantidade -= 1;
            await _livroRepository.UpdateAsync(livro);

            var emprestimo = ObjectMapper.Map<Emprestimo>(emprestimoDto);
            emprestimo.DataEmprestimo = DateTime.Now;
            await Repository.InsertAsync(emprestimo);
            return MapToEntityDto(emprestimo);
        }

        public async Task<PagedResultDto<EmprestimoDto>> GetAllEmprestimos(PagedEmprestimoResultRequestDto input, int pageNumber, int pageSize)
        {
            if (input.Keyword == null) input.Keyword = "";
            var emprestimos = Repository.GetAll()
                .Include(x => x.Usuario)
                .Include(x => x.Livro)
                .Where(x => x.Livro.Titulo.Contains(input.Keyword) || x.Usuario.Name.Contains(input.Keyword));

            var items = emprestimos.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize).ToList();

            var emprestimosDto = ObjectMapper.Map<List<EmprestimoDto>>(items);
            foreach (var item in emprestimosDto)
            {
                var emprestimoMap = emprestimos.FirstOrDefault(x => x.Id == item.Id);
                item.TituloLivro = emprestimoMap.Livro.Titulo;
                item.NomeUsuario = emprestimoMap.Usuario.Name;
            }
            return new PagedResultDto<EmprestimoDto>(emprestimosDto.Count(), emprestimosDto);
        }
    }
}
