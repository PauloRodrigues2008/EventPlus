using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Repositories;

public class TipoEventoRepository : ITipoEventoRepository
{
    private readonly EventContext _context;

    //Injeção de dependência: Recebe o contexto pelo construtor
    public TipoEventoRepository(EventContext context)
        { 
        _context = context;
       }

   /// <summary>
   /// Atualiza um tipo de evento usando o rastreamento automatico
   /// </summary>
   /// <param name="id"></param>
   /// <param name="tipoEvento"></param>
  
    public void Atualizar(Guid id, TipoEvento tipoEvento)
    {
        var TipoEventoBuscado = _context.TipoEventos.Find(id);
        if (TipoEventoBuscado != null)
        {
         TipoEventoBuscado.Titulo = tipoEvento.Titulo;
            // o SaveChanges() detecta as mudanças na propriedade "Titulo" automaticamente
            _context.SaveChanges();
        }
    }
    /// <summary>
    /// Busca um tipo de evento por id
    /// </summary>
    /// <param name="id">id do tipo evento a ser buscado</param>
    /// <returns> Objeto do tipoEvento com as informações do tipo de evento buscado</returns>

    public TipoEvento BuscarPorId(Guid id)
    {
        return _context.TipoEventos.Find(id)!;
    }


    /// <summary>
    /// Cadastrar um novo tipo de evento
    /// </summary>
    /// <param name="tipoEvento"> tipo de evento a ser cadastrado</param>
    public void Cadastrar(TipoEvento tipoEvento)
    {
        _context.TipoEventos.Add(tipoEvento);
        _context.SaveChanges();
    }

    /// <summary>
    /// Deleta um tipo de evento 
    /// </summary>
    /// <param name="id">id do tipo evento a ser deletado</param>

    public void Deletar(Guid id)
    {
      var tipoEventoBuscado = _context.TipoEventos.Find(id);
        if (tipoEventoBuscado != null)
        {
            _context.TipoEventos.Remove(tipoEventoBuscado);
            _context.SaveChanges();
        }
    }


    /// <summary>
    /// Busca a lista de tipo de eventos cadastrado
    /// </summary>
    /// <returns>Uma lista de tipo eventos</returns>
    public List<TipoEvento> Listar()
    {
        return _context.TipoEventos
            .OrderBy(tipoEvento => tipoEvento.Titulo)
            .ToList();
    }
}

