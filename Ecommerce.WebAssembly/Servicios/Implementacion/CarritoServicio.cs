using Blazored.LocalStorage;
using Blazored.Toast;
using Blazored.Toast.Services;
using Ecommerce.DTO;
using Ecommerce.WebAssembly.Servicios.Contrato;

namespace Ecommerce.WebAssembly.Servicios.Implementacion
{
    public class CarritoServicio : ICarritoServicio
    {
        private ILocalStorageService _localStorageService;
        private ISyncLocalStorageService _syncLocalStorageService;
        private IToastService _toastService;

        public CarritoServicio
        (
            ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService,
            IToastService toastService
        )
        {
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _toastService = toastService;
        }

        public event Action? MostrarItems;

        public async Task AgregarCarrito(CarritoDTO modelo)
        {
            try
            {
                // Devuelve la lista del carrito, en caso de ser null crea una nueva
                var carrito = await _localStorageService.GetItemAsync<List<CarritoDTO>>("carrito") ?? new List<CarritoDTO>();
                // Busca a ver si el producto ya existe en la lista
                var encontrado = carrito.FirstOrDefault(c => c.Producto!.IdProducto == modelo.Producto!.IdProducto);
                // Si existe lo elimina
                if (encontrado != null)
                    carrito.Remove(encontrado);
                // Agrega el producto a la lista
                carrito.Add(modelo);
                // Actualizando el objeto carrito
                await _localStorageService.SetItemAsync("carrito", carrito);
                // Se manejan los monsajes
                if (encontrado != null)
                    _toastService.ShowSuccess("El producto fue actualizado");
                else
                    _toastService.ShowSuccess("El producto fue agregado al carrito");
                // Se actualiza la vista (# de productos en el carrito)
                MostrarItems?.Invoke();
            }
            catch (Exception)
            {
                _toastService.ShowError("No se pudo agregar al carrito");
            }
        }

        public int CantidadProductos()
        {
            var carrito = _syncLocalStorageService.GetItem<List<CarritoDTO>>("carrito");
            return carrito == null ? 0 : carrito.Count;
        }

        public async Task<List<CarritoDTO>> DevolverCarrito()
        {
            var carrito = await _localStorageService.GetItemAsync<List<CarritoDTO>>("carrito") ?? new List<CarritoDTO>();
            return carrito;
        }

        public async Task EliminarProducto(int idProducto)
        {
            try
            {
                var carrito = await _localStorageService.GetItemAsync<List<CarritoDTO>>("carrito");

                if (carrito != null)
                {
                    var elemento = carrito.FirstOrDefault(c => c.Producto!.IdProducto == idProducto);
                    if (elemento != null)
                    {
                        carrito.Remove(elemento);
                        await _localStorageService.SetItemAsync("carrito", carrito);
                        MostrarItems?.Invoke();
                        _toastService.ShowSuccess("Se ha bajado este producto del carro");
                    }
                }

            }
            catch (Exception)
            {
                _toastService.ShowError("Ha ocurrido un problema al eliminar este producto del carro");
            }
        }

        public async Task LimpiarCarrito()
        {
            await _localStorageService.RemoveItemAsync("carrito");
            MostrarItems?.Invoke();
        }
    }
}
