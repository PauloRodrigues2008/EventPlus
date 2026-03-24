using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace filmes.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]


public class LoginController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;

    }
    [HttpPost]
    public IActionResult Login(LoginDTO loginDto)
    {
        try
        {
            Usuario UsuarioBuscado = _usuarioRepository.BuscarPorEmailESenha(loginDto.Email, loginDto.Senha);

            if (UsuarioBuscado == null)
            {
                return NotFound("Email ou senha invalidos.");
            }


            var claims = new[]
            {
                   //Caso encontre o usuário, prosseguir para criação do token
                   
                  //1* - Definir as informações(Claims) que serão fornecidas no token (Payload) var


                    //formato da claim
                  

                    new Claim(JwtRegisteredClaimNames.Email, UsuarioBuscado.Email),

                     new Claim(JwtRegisteredClaimNames.Name, UsuarioBuscado.Nome!),

                       new Claim(JwtRegisteredClaimNames.Jti, UsuarioBuscado.IdUsuario.ToString()),

                      new Claim(ClaimTypes.Role, UsuarioBuscado.IdTipoUsuarioNavigation!.Titulo!)


                      //existe a possibilidade de criar uma claim personalizada
                      //new Claim("Claim Persolizada", "Valor da claim personalizada")
                };

            //2* - Definir a chave de acesso ao token
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("filmes-chave-autenticacao-webapi-dev"));

            //3* Definir as caracteristicas do token (HEADER)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //4* - Gerar token
            var token = new JwtSecurityToken
            (
                //emissor do token
                issuer: "api_event",

                //destinatário do token
                audience: "api_event",

                //dados definidos nas claims(informações)
                claims: claims,

                //tempo de expiração do token
                expires: DateTime.Now.AddMinutes(5),

                //credencias do token
                signingCredentials: creds
            );

            //5* - Retornar o token criado
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
        catch (Exception)
        {

            throw;
        }
    }

}

