﻿using AspNetCore20Example.Application.ViewModels;
using AspNetCore20Example.Services.API.Integration.Tests.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore20Example.Services.API.Integration.Tests.Controllers
{
    public class ContatosControllerIntegrationTest
    {
        public ContatosControllerIntegrationTest()
        {
            Environment.CriarServidor();
        }

        [Fact]
        public async Task Novo_ContatoValido_DeveRetornar200()
        {
            var login = await Utils.RealizarLoginOrganizador(Environment.Client);

            var contato = new ContatoViewModel
            {
                Nome = "Teste",
                Email = "teste@teste.com",
                Endereco = new EnderecoViewModel
                {
                    Logradouro = "Rua 1",
                    Bairro = "bairro",
                    CEP = "14778665",
                    Cidade = "cidade",
                    Estado = "SP",
                    Numero = "123"
                }
            };

            var response = await Environment.Server
                .CreateRequest("api/contatos")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(req => req.Content = Utils.GerarRequestContent(contato))
                .PostAsync();

            var contatoResult = JsonConvert.DeserializeObject<ContatoJsonDTO>(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
            Assert.Equal(contatoResult.data.nome, contato.Nome);
            Assert.NotEmpty(contatoResult.data.id);
            Assert.NotNull(contatoResult.data.id);
            Assert.NotEmpty(contatoResult.data.enderecoId);
            Assert.NotNull(contatoResult.data.enderecoId);
        }
    }
}