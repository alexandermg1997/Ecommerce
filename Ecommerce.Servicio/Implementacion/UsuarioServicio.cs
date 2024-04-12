using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Ecommerce.DTO;
using Ecommerce.Modelo;
using Ecommerce.Repositorio.Contrato;
using Ecommerce.Servicio.Contrato;
using AutoMapper;

namespace Ecommerce.Servicio.Implementacion
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IGenericoRepositorio<Usuario> _modeloRepositorio;
        private readonly IMapper _mapper;

        public UsuarioServicio(IGenericoRepositorio<Usuario> modeloRepositorio, IMapper mapper)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
        }
        public async Task<SesionDTO> Autorizacion(LoginDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.Correo == modelo.Correo && p.Clave == modelo.Clave);
                var fromDdModelo = await consulta.FirstOrDefaultAsync();

                return fromDdModelo != null
                    ? _mapper.Map<SesionDTO>(fromDdModelo)
                    : throw new TaskCanceledException("No se encontraron coincidencias");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Usuario>(modelo);
                var rspModelo = await _modeloRepositorio.Crear(dbModelo);

                return rspModelo.IdUsuario != 0
                    ? _mapper.Map<UsuarioDTO>(rspModelo)
                    : throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.IdUsuario == modelo.IdUsuario);
                var fromDbModelo = await consulta.FirstOrDefaultAsync();

                if (fromDbModelo != null)
                {
                    fromDbModelo.NombreCompleto = modelo.NombreCompleto;
                    fromDbModelo.Correo = modelo.Correo;
                    fromDbModelo.Clave = modelo.Clave;
                    var respuesta = await _modeloRepositorio.Editar(fromDbModelo);

                    return !respuesta
                        ? throw new TaskCanceledException("No se pudo editar")
                        : respuesta;
                }
                else
                {
                    throw new TaskCanceledException("No se encontraron resultados");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.IdUsuario == id);
                var fromDbModelo = await consulta.FirstOrDefaultAsync();

                if (fromDbModelo != null)
                {
                    var respuesta = await _modeloRepositorio.Eliminar(fromDbModelo);

                    return !respuesta
                        ? throw new TaskCanceledException("No se pudo eliminar")
                        : respuesta;
                }
                else
                {
                    throw new TaskCanceledException("No se encontraron resultados");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<UsuarioDTO>> Lista(string rol, string buscar)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.Rol == rol
                && string.Concat(p.NombreCompleto.ToLower(), p.Correo.ToLower()).Contains(buscar.ToLower()));

                List<UsuarioDTO> lista = _mapper.Map<List<UsuarioDTO>>(await consulta.ToListAsync());

                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UsuarioDTO> Obtener(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.IdUsuario == id);
                var fromDbModelo = await consulta.FirstOrDefaultAsync();

                return fromDbModelo != null
                    ? _mapper.Map<UsuarioDTO>(fromDbModelo)
                    : throw new TaskCanceledException("No se encontraron coincidencias");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
