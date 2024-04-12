using AutoMapper;
using Ecommerce.DTO;
using Ecommerce.Modelo;
using Ecommerce.Repositorio.Contrato;
using Ecommerce.Servicio.Contrato;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Servicio.Implementacion
{
    public class ProductoServicio : IProductoServicio
    {

        private readonly IGenericoRepositorio<Producto> _modeloRepositorio;
        private readonly IMapper _mapper;

        public ProductoServicio(IGenericoRepositorio<Producto> modeloRepositorio, IMapper mapper)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ProductoDTO>> Catalogo(string categoria, string buscar)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p =>
                p.Nombre.ToLower().Contains(buscar.ToLower()) &&
                p.IdCategoriaNavigation.Nombre.ToLower().Contains(categoria.ToLower()));

                List<ProductoDTO> lista = _mapper.Map<List<ProductoDTO>>(await consulta.ToListAsync());

                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Producto>(modelo);
                var rspModelo = await _modeloRepositorio.Crear(dbModelo);

                return rspModelo.IdProducto != 0
                    ? _mapper.Map<ProductoDTO>(rspModelo)
                    : throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.IdProducto == modelo.IdProducto);
                var fromDbModelo = await consulta.FirstOrDefaultAsync();

                if (fromDbModelo != null)
                {
                    fromDbModelo.Nombre = modelo.Nombre;
                    fromDbModelo.Descripcion = modelo.Descripcion;
                    fromDbModelo.IdCategoria = modelo.IdCategoria;
                    fromDbModelo.Precio = modelo.Precio;
                    fromDbModelo.PrecioOferta = modelo.PrecioOferta;
                    fromDbModelo.Cantidad = modelo.Cantidad;
                    fromDbModelo.Imagen = modelo.Imagen;
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
                var consulta = _modeloRepositorio.Consultar(p => p.IdProducto == id);
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

        public async Task<List<ProductoDTO>> Lista(string buscar)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.Nombre.ToLower().Contains(buscar.ToLower()));

                consulta = consulta.Include(c => c.IdCategoriaNavigation);

                List<ProductoDTO> lista = _mapper.Map<List<ProductoDTO>>(await consulta.ToListAsync());

                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProductoDTO> Obtener(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.IdProducto == id);

                consulta = consulta.Include(c => c.IdCategoriaNavigation);

                var fromDbModelo = await consulta.FirstOrDefaultAsync();

                return fromDbModelo != null
                    ? _mapper.Map<ProductoDTO>(fromDbModelo)
                    : throw new TaskCanceledException("No se encontraron coincidencias");
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
