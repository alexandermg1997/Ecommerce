using AutoMapper;
using Ecommerce.DTO;
using Ecommerce.Modelo;
using Ecommerce.Repositorio.Contrato;
using Ecommerce.Servicio.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Servicio.Implementacion
{
    public class VentaServicio: IVentaServicio
    {
        private readonly IVentaRepositorio _modeloRepositorio;
        private readonly IMapper _mapper;

        public VentaServicio(IVentaRepositorio modeloRepositorio, IMapper mapper)
        {
            _modeloRepositorio = modeloRepositorio;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var dbModelo = _mapper.Map<Venta>(modelo);
                var ventaGenerada = await _modeloRepositorio.Registrar(dbModelo);

                return ventaGenerada.IdVenta != 0
                    ? _mapper.Map<VentaDTO>(ventaGenerada)
                    : throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
