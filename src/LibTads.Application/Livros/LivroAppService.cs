using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using LibTads.Authorization;
using LibTads.Autores.Dto;
using LibTads.Domain;
using LibTads.Livros.Dto;
using LibTads.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using QRCoder;

namespace LibTads.Livros
{
    [AbpAuthorize(PermissionNames.Pages_Livros)]
    public class LivroAppService : LibTadsAppServiceBase
    {
        private readonly IRepository<Livro, int> _repository;
        private readonly IRepository<Autor, int> _repositoryAutor;
        private readonly IRepository<Genero, int> _repositoryGenero;
        private readonly IRepository<Emprestimo, int> _repositoryEmprestimo;
        public LivroAppService(
            IRepository<Livro, int> repository,
            IRepository<Autor, int> repositoryAutor,
            IRepository<Genero, int> repositoryGenero,
            IRepository<Emprestimo, int> repositoryEmprestimo
        )
        {
            _repository = repository;
            _repositoryAutor = repositoryAutor;
            _repositoryGenero = repositoryGenero;
            _repositoryEmprestimo = repositoryEmprestimo;
        }

        public async Task<LivroDto> CreateAsync(CreateLivroDto livroDto)
        {
            if (livroDto.Isbn != null)
            {
                var haveLivro = await _repository.FirstOrDefaultAsync(x => x.Isbn.Equals(livroDto.Isbn));
                if (haveLivro != null)
                {
                    if (haveLivro.IsDeleted)
                    {
                        haveLivro.IsDeleted = false;
                        await _repository.UpdateAsync(haveLivro);
                        return ObjectMapper.Map<LivroDto>(haveLivro);
                    }
                    throw new UserFriendlyException("Esse livro já está cadastrado");
                }
            }

            var livro = ObjectMapper.Map<Livro>(livroDto);
            livro.CreationTime = DateTime.Now;
            var livroId = await _repository.InsertAndGetIdAsync(livro);
            var livroMap = ObjectMapper.Map<LivroDto>(livro);
            if (livroDto.GerarQrCode)
            {
                livro.QrCode = gerarQrCode(livroId);
                livroMap.QrCode = livro.QrCode;
                await _repository.UpdateAsync(livro);
            }
            return livroMap;
        }

        private string gerarQrCode(int id)
        {
            var url = "http://localhost:4200/app/emprestimos/create-emprestimo/" + id;
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(5);

                using (MemoryStream stream = new MemoryStream())
                {
                    qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
        }

        public string CadastrarQrCode(int idLivro)
        {
            var livro = _repository.FirstOrDefault(x => x.Id.Equals(idLivro));
            livro.QrCode = gerarQrCode(idLivro);
            return livro.QrCode;
        }

        public async Task<LivroDto> UpdateAsync(UpdateLivroDto livroDto)
        {
            if (livroDto.Isbn != null)
            {
                var haveIsbn = _repository.FirstOrDefault(x => x.Isbn.Equals(livroDto.Isbn) && x.Id != livroDto.Id);
                if (haveIsbn != null)
                {
                    throw new UserFriendlyException("Esse isbn esta vinculado a outro livro!");
                }
            }
            var livro = ObjectMapper.Map<Livro>(livroDto);
            await _repository.UpdateAsync(livro);
            return ObjectMapper.Map<LivroDto>(livro);
        }

        public async Task DeActivate(int idLivro)
        {
            var livro = await _repository.FirstOrDefaultAsync(x => x.Id == idLivro);
            livro.IsDeleted = true;
            await _repository.UpdateAsync(livro);
        }

        public async Task<PagedResultDto<LivroDto>> GetLivros(PagedLivroResultRequestDto input, int pageNumber, int pageSize)
        {
            if (input.Keyword == null) input.Keyword = "";
            var query = _repository.GetAllListAsync(e => e.IsDeleted == false && (e.Titulo.Contains(input.Keyword) || e.Autor.Nome.Contains(input.Keyword) || e.Genero.Descricao.Contains(input.Keyword)));

            var totalCount = query.Result.Count;
            var items = query.Result.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize).ToList();

            var itemDtos = ObjectMapper.Map<List<LivroDto>>(items);

            foreach (var item in itemDtos)
            {
                var autor = await _repositoryAutor.FirstOrDefaultAsync(x => x.Id == item.AutorId);
                var genero = await _repositoryGenero.FirstOrDefaultAsync(x => x.Id == item.GeneroId);
                item.EmprestimosAndamento = _repositoryEmprestimo.GetAll().Where(x => x.LivroId.Equals(item.Id) && x.DataDevolucao == null).Count();
                item.DescricaoGenero = genero.Descricao;
                item.NomeAutor = autor.Nome;
            }

            return new PagedResultDto<LivroDto>(totalCount, itemDtos);
        }

        public async Task<LivroDto> GetLivroById(int id)
        {
            var livro = await _repository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return ObjectMapper.Map<LivroDto>(livro);
        }
    }
}
