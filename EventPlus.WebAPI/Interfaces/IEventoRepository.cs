using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

    public interface IEventoRepository
    {
    void cadastrar(Evento evento);
    List<Evento> Listar();
    void Deletar(Guid id);
    void Atualizar(Guid id, Evento evento);
    List<Evento> ListarPorId(Guid IdUsuario);
    List<Evento> ProximosEventos();
    Evento BuscarPorId(Guid id);

    }

