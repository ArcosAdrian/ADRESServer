using AdresLib.Entidades;
using Microsoft.Data.Sqlite;

namespace AdresLib
{
    public interface IAdresService
    {
        string Ping();

        List<IdNombre> ProveedoresListar();
        List<IdNombre> ServiciosListar();
        List<IdNombre> UnidadesListar();

        AdquisicionExtra AdquisicionesRecuperar(long id);
        List<AdquisicionExtra> AdquisicionesListar();
        void AdquisicionAdicionar(Adquisicion adquisicion);
        void AdquisicionModificar(Adquisicion adquisicion);
        void AdquisicionEliminar(long id);


        void AdquisicionHistoriaAdicionar(Adquisicion adquisicionAnterior, Adquisicion adquisicionNueva, Accion acion, SqliteConnection conn);
        List<AdquisicionHistoria> AdquisicionesHistoriaListarPorId(int id);
    }
}
