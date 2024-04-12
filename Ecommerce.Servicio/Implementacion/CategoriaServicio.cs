﻿using AutoMapper;
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
    public class CategoriaServicio: ICategoriaServicio
    {
        private readonly IGenericoRepositorio<Categoria> _modeloRepositorio;
        private readonly IMapper _mapper;

        public CategoriaServicio(IGenericoRepositorio<Categoria> modeloRepositorio, IMapper mapper)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
        }
       
        public async Task<CategoriaDTO> Crear(CategoriaDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Categoria>(modelo);
                var rspModelo = await _modeloRepositorio.Crear(dbModelo);

                return rspModelo.IdCategoria != 0
                    ? _mapper.Map<CategoriaDTO>(rspModelo)
                    : throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Editar(CategoriaDTO modelo)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.IdCategoria == modelo.IdCategoria);
                var fromDbModelo = await consulta.FirstOrDefaultAsync();

                if (fromDbModelo != null)
                {
                    fromDbModelo.Nombre = modelo.Nombre;
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
                var consulta = _modeloRepositorio.Consultar(p => p.IdCategoria == id);
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

        public async Task<List<CategoriaDTO>> Lista(string buscar)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.Nombre.ToLower().Contains(buscar.ToLower()));

                List<CategoriaDTO> lista = _mapper.Map<List<CategoriaDTO>>(await consulta.ToListAsync());

                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CategoriaDTO> Obtener(int id)
        {
            try
            {
                var consulta = _modeloRepositorio.Consultar(p => p.IdCategoria == id);
                var fromDbModelo = await consulta.FirstOrDefaultAsync();

                return fromDbModelo != null
                    ? _mapper.Map<CategoriaDTO>(fromDbModelo)
                    : throw new TaskCanceledException("No se encontraron coincidencias");
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}