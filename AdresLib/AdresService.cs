using AdresLib.Entidades;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdresLib
{
    public class AdresService : ServiceBase, IAdresService
    {
        string version = "1.0.0";
        public string Ping()
        {
            return $"[AdresService] Pong {version}, {DateTime.Now} ";
        }
        public SqliteConnection getConnection()
        {
            return GetConnection();
        }

        public List<IdNombre> ProveedoresListar()
        {
            return IdNombreListar("SELECT id, nombre FROM proveedor ORDER BY nombre;");
        }

        public List<IdNombre> ServiciosListar()
        {
            return IdNombreListar("SELECT id, nombre FROM servicio ORDER BY nombre;");
        }

        public List<IdNombre> UnidadesListar()
        {
            return IdNombreListar("SELECT id, nombre FROM unidad ORDER BY nombre;");
        }

        #region Adquisicion

        
        public List<AdquisicionExtra> AdquisicionesListar()
        {
            
            return Listar<AdquisicionExtra>("SELECT * FROM vta_adquisicion_completo;");
        }
        public AdquisicionExtra AdquisicionesRecuperar(long id)
        {
            const string sql = @"SELECT * FROM vta_adquisicion_completo WHERE id = @Id;";
            using (var conn = GetConnection())
            {
                conn.Open();
                var result = conn.QuerySingleOrDefault<AdquisicionExtra>(sql, new { Id = id });
                if (result == null)
                {
                    throw new InvalidOperationException($"No existe la Adquisicion para el id {id}");
                }
                return result;
            }
        }

        public void AdquisicionAdicionar(Adquisicion adquisicion)
        {
            
            AdresServiceUtil.AdquisicionValidar(adquisicion, Accion.Adicionar);

            const string sql = @"
            INSERT INTO adquisicion (
                proveedorId,
                servicioId,
                unidadId,
                presupuesto,
                cantidad,
                valorUnitario,
                fechaAdquisicion,
                comentario
            )
            VALUES (
                @ProveedorId,
                @ServicioId,
                @UnidadId,
                @Presupuesto,
                @Cantidad,
                @ValorUnitario,
                @FechaAdquisicion,
                @Comentario
            )
            RETURNING Id;";

            using (var conn = GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    
                    var id = conn.ExecuteScalar<long>(sql, new
                    {
                        adquisicion.ProveedorId,
                        adquisicion.ServicioId,
                        adquisicion.UnidadId,
                        adquisicion.Presupuesto,
                        adquisicion.Cantidad,
                        adquisicion.ValorUnitario,
                        adquisicion.FechaAdquisicion,
                        adquisicion.Comentario
                    });

                    adquisicion.Id = id;
                    AdquisicionHistoriaAdicionar(null, adquisicion, Accion.Adicionar, conn);
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                
            }
        }
        /// <summary>
        /// Actualiza los detalles de una adquisición existente.
        /// </summary>
        /// <remarks>Este método modifica las propiedades de una adquisición existente basándose en el objeto proporcionado.
        /// Asegúrate de que el parámetro <paramref name="adquisicion"/> contenga datos válidos y completos antes de
        /// llamar a este método.</remarks>
        /// <param name="adquisicion">El objeto de adquisición que contiene los detalles actualizados. No puede ser nulo.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void AdquisicionModificar(Adquisicion adquisicion)
        {
            
            AdquisicionExtra adquisicionAnterior = AdquisicionesRecuperar(adquisicion.Id.Value);
           
            AdresServiceUtil.AdquisicionValidar(adquisicion, Accion.Modificar);

            const string sql = @"
            UPDATE adquisicion SET
                proveedorId = @ProveedorId,
                servicioId = @ServicioId,
                unidadId = @UnidadId,
                presupuesto = @Presupuesto,
                cantidad = @Cantidad,
                valorUnitario = @ValorUnitario,
                fechaAdquisicion = @FechaAdquisicion,
                comentario = @Comentario
            WHERE Id = @Id;";

            using (var conn = GetConnection())
            {
                
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new
                    {
                        adquisicion.ProveedorId,
                        adquisicion.ServicioId,
                        adquisicion.UnidadId,
                        adquisicion.Presupuesto,
                        adquisicion.Cantidad,
                        adquisicion.ValorUnitario,
                        adquisicion.FechaAdquisicion,
                        adquisicion.Comentario,
                        adquisicion.Id
                    });

                    AdquisicionHistoriaAdicionar(adquisicionAnterior, adquisicion, Accion.Modificar, conn);
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                
            }
        }

        
        public void AdquisicionEliminar(long id)
        {
            AdquisicionExtra adquisicionAnterior = AdquisicionesRecuperar(id);
            if (adquisicionAnterior == null) {
                throw new Exception("La adquisición no existe.");
            }
                
            

            const string sql = @"
            DELETE FROM adquisicion WHERE Id = @Id;";

            using (var conn = GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                try
                {
                    conn.Execute(sql, new { Id = id });

                    AdquisicionHistoriaAdicionar(adquisicionAnterior, null, Accion.Eliminar, conn);
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        #endregion

        #region AdquisicionHistoria
        public void AdquisicionHistoriaAdicionar(Adquisicion adquisicionAnterior, Adquisicion adquisicionNueva, Accion tipoCambio, SqliteConnection conn)
        {
   
            AdquisicionHistoria adquisicion = new AdquisicionHistoria();
            if (tipoCambio != Accion.Adicionar)
            {
                adquisicion.AdquisicionId = adquisicionAnterior.Id;
                adquisicion.ServicioId = adquisicionAnterior.ServicioId;
                adquisicion.UnidadId = adquisicionAnterior.UnidadId;
                adquisicion.ProveedorId = adquisicionAnterior.ProveedorId;
                adquisicion.Presupuesto = adquisicionAnterior.Presupuesto;
                adquisicion.Cantidad = adquisicionAnterior.Cantidad;
                adquisicion.ValorUnitario = adquisicionAnterior.ValorUnitario;
                adquisicion.FechaAdquisicion = adquisicionAnterior.FechaAdquisicion;
                adquisicion.Comentario = adquisicionAnterior.Comentario;

            }
            else {
                adquisicion.AdquisicionId = adquisicionNueva.Id;
            }

            var cambios = AdresServiceUtil.AdquisicionObtenerCambios(adquisicionAnterior, adquisicionNueva);
            if (cambios.Count == 0)
            {
                return;
            }

            adquisicion.Cambios = string.Join("; ", cambios);
            adquisicion.FechaCambio = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            adquisicion.TipoCambio = tipoCambio.ToString();
            adquisicion.UsuarioCambio = AdresServiceUtil.ObtenerUsuario();


            if (string.IsNullOrEmpty(adquisicion.UsuarioCambio))
            {
                throw new ArgumentException("El usuario no puede ser nulo o vacio");
            }

            const string sql = @"
            INSERT INTO adquisicionHistoria (
                adquisicionId,
                proveedorId,
                servicioId,
                unidadId,
                presupuesto,
                cantidad,
                valorUnitario,
                fechaAdquisicion,
                comentario,
                fechaCambio, 
                usuarioCambio, 
                tipoCambio,
                cambios
            )
            VALUES (
                @AdquisicionId,                
                @ProveedorId,
                @ServicioId,
                @UnidadId,
                @Presupuesto,
                @Cantidad,
                @ValorUnitario,
                @FechaAdquisicion,
                @Comentario,
                @FechaCambio,
                @UsuarioCambio,
                @TipoCambio,
                @Cambios
            )
            RETURNING Id;";

         
            conn.Open();
            var id = conn.ExecuteScalar<long>(sql, new
            {
                adquisicion.AdquisicionId,
                adquisicion.ProveedorId,
                adquisicion.ServicioId,
                adquisicion.UnidadId,
                adquisicion.Presupuesto,
                adquisicion.Cantidad,
                adquisicion.ValorUnitario,
                adquisicion.FechaAdquisicion,
                adquisicion.Comentario,
                adquisicion.FechaCambio,
                adquisicion.UsuarioCambio,
                adquisicion.TipoCambio,
                adquisicion.Cambios
            });
            
        }
        public List<AdquisicionHistoria> AdquisicionesHistoriaListarPorId(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                return conn.Query<AdquisicionHistoria>("select * from adquisicionHistoria where adquisicionId = @Id;", new { Id = id }).ToList();
            }
        }
        #endregion
    }
}
