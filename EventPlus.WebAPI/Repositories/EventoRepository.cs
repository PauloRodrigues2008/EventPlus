using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

public class EventoRepository : IEventoRepository
{
    private readonly EventContext _context;

    public EventoRepository(EventContext context)
    {
        _context = context;
    }

    public void Atualizar(Guid id, Evento evento)
    {
        var EventoBuscado = _context.Eventos.Find(id);

        if (EventoBuscado != null)
        {
            EventoBuscado.Presencas = evento.Presencas;
            EventoBuscado.Descricao = evento.Descricao;
            EventoBuscado.DataEvento = evento.DataEvento;
            EventoBuscado.Nome = evento.Nome;
            EventoBuscado.IdInstituicaoNavigation = evento.IdInstituicaoNavigation;
            EventoBuscado.IdTipoEventoNavigation = evento.IdTipoEventoNavigation;
            //O SaveChanges() detecta as mudanças na propriedade "Titulo" automaticamente
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Método que busca um evento pelo id
    /// </summary>
    /// <param name="id">id do evento a ser buscado</param>
    /// <returns>Objeto do evento com as informações do evento buscado</returns>
    public Evento BuscarPorId(Guid id)
    {
        return _context.Eventos.Find(id)!;
    }

    /// <summary>
    /// Método que cadastra um evento
    /// </summary>
    /// <param name="evento">evento a ser cadastrado</param>
    public void Cadastrar(Evento evento)
    {
        _context.Eventos.Add(evento);
        _context.SaveChanges();
    }

    /// <summary>
    /// Método que deleta um evento
    /// </summary>
    /// <param name="IdEvento">id do evento a ser deletado</param>
    public void Deletar(Guid Id)
    {
        var eventoBuscado = _context.Eventos.Find(Id);

        if (eventoBuscado != null)
        {
            _context.Eventos.Remove(eventoBuscado);
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Método que busca todos os eventos
    /// </summary>
    /// <returns></returns>
    public List<Evento> Listar()
    {
        return _context.Eventos
            .Include(e => e.IdTipoEventoNavigation)
            .Include(e => e.IdInstituicaoNavigation)
            .ToList();
    }

    /// <summary>
    /// Método que busca eventos no qual um usuário confirmou presença
    /// </summary>
    /// <param name="IdUsuario">Id do usuário a ser buscado</param>
    /// <returns>Uma lista de eventos</returns>
    public List<Evento> ListarPorId(Guid IdUsuario)
    {
        return _context.Eventos
            .Include(e => e.IdTipoEventoNavigation)
            .Include(e => e.IdInstituicaoNavigation)
            .Where(e => e.Presencas.Any(p => p.IdUsuario == IdUsuario && p.Situacao == true)).ToList();
    }

    /// <summary>
    /// Método que traz a lista de próximos eventos
    /// </summary>
    /// <returns>Uma lista de eventos</returns>
    public List<Evento> ProximosEventos()
    {
        return _context.Eventos
            .Include(e => e.IdTipoEventoNavigation)
            .Include(e => e.IdInstituicaoNavigation)
            .Where(e => e.DataEvento >= DateTime.Now)
            .OrderBy(e => e.DataEvento)
            .ToList();
    }
}