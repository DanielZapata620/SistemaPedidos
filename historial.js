const historialVentas = JSON.parse(localStorage.getItem("historial")) || [];
const tablaBody = document.querySelector(".tabla tbody");
const ventasTotales = document.getElementById("ventas");
const totalRecaudado = document.getElementById("recaudado");

function CargarHistorialMemoria() {
    let contadorVentas = 0;
    let sumaTotal = 0;

    if (historialVentas.length > 0) {

        historialVentas.forEach(venta => {


            const fila = document.createElement("tr");
            const fecha = document.createElement("td");
            const productos = document.createElement("td");
            const medioPago = document.createElement("td");
            const total = document.createElement("td");


            fecha.textContent = venta.fecha;

            const listaProductos = document.createElement("ul");

            venta.productos.forEach(producto => {
                const item = document.createElement("li");
                item.textContent = `${producto.nombre} x${producto.cantidad}`;
                listaProductos.appendChild(item);
            });

            productos.appendChild(listaProductos);
            medioPago.textContent = venta.medioPago;
            total.textContent = venta.monto

            sumaTotal += parseFloat(venta.monto);
            contadorVentas++;

            fila.appendChild(fecha);
            fila.appendChild(productos);
            fila.appendChild(medioPago);
            fila.appendChild(total);


            tablaBody.appendChild(fila);


        }

        )
    }

    ventasTotales.textContent = contadorVentas;
    totalRecaudado.textContent = `$${sumaTotal}`;
}

CargarHistorialMemoria();