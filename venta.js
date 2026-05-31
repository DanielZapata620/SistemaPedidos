//Contraseña para cancelar venta: 123

const catalogo = document.querySelector(".Catalago");
const Tabla = document.querySelector("table")
const tbody = Tabla.querySelector("tbody");
const btnCancelar = document.querySelector(".btnCancelar");
const btnPagar = document.querySelector(".btnRealizarVenta");
const inputCodigo = document.querySelector("#cantidad");
const padNumerico = document.querySelector(".pad-numerico");
const btnLimpiar = document.querySelector("#limpiarInput");

let ticketMemoria = [];


catalogo.addEventListener("click", function (e) {

  const productoSeleccionado = e.target.closest(".producto");
  btnCancelar.style.display = "inline";
  btnPagar.style.display = "inline";


  if (productoSeleccionado) {

    const ArregloNombresProducto = Array.from(Tabla.querySelectorAll("tr td:first-child"));
    console.log(ArregloNombresProducto);

    const ProductoEncontradoTicket = ArregloNombresProducto.find(n => n.textContent == productoSeleccionado.querySelector(".Nombre").textContent)
    const pCantidad = productoSeleccionado.querySelector(".cantidad");
    console.log(ProductoEncontradoTicket);

    if (ProductoEncontradoTicket) {
      const filaEditar = ProductoEncontradoTicket.parentElement;
      const filaCantidad = filaEditar.querySelector("td:nth-child(2)");
      const filaPTotal = filaEditar.querySelector("td:nth-child(4)");
      let cantidad = parseInt(filaCantidad.textContent) + 1;

      pCantidad.textContent = cantidad;


      PrecioTotalPorProducto = parseFloat(filaEditar.querySelector("td:nth-child(3)").textContent) * cantidad;

      filaCantidad.textContent = cantidad
      filaPTotal.textContent = PrecioTotalPorProducto;

      const productoTicketMemoria = ticketMemoria.find(p => p.nombre === filaEditar.querySelector("td:nth-child(1)").textContent);
      console.log(productoTicketMemoria)
      productoTicketMemoria.cantidad = cantidad;
      productoTicketMemoria.precioTotal = PrecioTotalPorProducto;


    }
    else {
      const fila = document.createElement("tr");
      const Nombre = document.createElement("td");
      const cantidad = document.createElement("td");
      const PUnit = document.createElement("td");
      const PTotal = document.createElement("td");


      Nombre.textContent = productoSeleccionado.querySelector(".Nombre").textContent;
      PUnit.textContent = parseFloat(productoSeleccionado.querySelector(".Precio").textContent.replace("$", ""));
      cantidad.textContent += 1;
      pCantidad.textContent = cantidad;
      pCantidad.textContent = cantidad.textContent;
      PTotal.textContent = parseFloat(cantidad.textContent * PUnit.textContent);

      ticketMemoria.push(
        {
          idProducto: productoSeleccionado.id,
          nombre: Nombre.textContent,
          cantidad: parseInt(cantidad.textContent),
          precioUnitario: parseFloat(PUnit.textContent),
          precioTotal: parseFloat(PTotal.textContent)
        }
      );



      fila.appendChild(Nombre)
      fila.appendChild(cantidad)
      fila.appendChild(PUnit)
      fila.appendChild(PTotal)

      tbody.appendChild(fila)
    }


    console.log("Ticket")
    console.log(ticketMemoria);
    localStorage.setItem("ticket", JSON.stringify(ticketMemoria));




    CalcularTotal();

  }
});

const modalCancelar = document.querySelector("#cancelarModal");
const error = document.querySelector("#errorModal");



btnCancelar.addEventListener("click", function () {

  modalCancelar.hidden = false;
  modalCancelar.parentElement.hidden = false;
  inputCodigo.value = "";
  error.textContent = "";
});

document.querySelector("#cerrarModalCancelar").addEventListener("click", function () {
  modalCancelar.hidden = true;
  modalCancelar.parentElement.hidden = true;
});

padNumerico.addEventListener("click", (e) => {
  if (e.target.tagName === "BUTTON") {
    const valor = e.target.textContent;
    inputCodigo.value += valor;

    if (inputCodigo.value.length > 0) {
      btnLimpiar.style.display = "inline";
    }
  }
});

btnLimpiar.addEventListener("click", function () {
  inputCodigo.value = "";
})

document.querySelector("#AceptarCancelacion").addEventListener("click", function () {


  let codigoCorrecto = "123"

  if (inputCodigo.value == codigoCorrecto) {
    const productos = catalogo.querySelectorAll(".producto .cantidad");

    let arregloProductos = Array.from(productos)
    arregloProductos.forEach(p => {
      p.textContent = "";
    })
    modalCancelar.hidden = true;
    modalCancelar.parentElement.hidden = true;
    error.textContent = "";
    localStorage.removeItem("ticket");

    tbody.querySelectorAll("tr").forEach(tr => {
      tbody.removeChild(tr);
    });

    ticketMemoria = [];
    p.textContent = "Total: $0";
    btnCancelar.style.display = "none";
    btnPagar.style.display = "none";

  }
  else {
    error.textContent = "El codigo de acceso es incorrecto. Solicite ayuda al gerente";
  }

});


function CargarTicketMemoria() {
  ticketMemoria = JSON.parse(localStorage.getItem("ticket")) || [];


  if (ticketMemoria.length > 0) {
    btnCancelar.style.display = "inline";
    btnPagar.style.display = "inline";

    ticketMemoria.forEach(producto => {
      const fila = document.createElement("tr");
      // let id=`#${producto.idProducto} .cantidad`;
      const pCantidad = document.querySelector(`#${producto.idProducto} .cantidad`);

      // console.log(producto.idProducto);
      // console.log(id)

      const tdNombre = document.createElement("td");
      const tdCantidad = document.createElement("td");
      const tdPUnit = document.createElement("td");
      const tdPTotal = document.createElement("td");

      pCantidad.textContent = producto.cantidad;
      tdNombre.textContent = producto.nombre;
      tdCantidad.textContent = producto.cantidad;
      tdPUnit.textContent = producto.precioUnitario;
      tdPTotal.textContent = producto.precioTotal;

      fila.appendChild(tdNombre);
      fila.appendChild(tdCantidad);
      fila.appendChild(tdPUnit);
      fila.appendChild(tdPTotal);
      tbody.appendChild(fila);


      CalcularTotal()
    })
  }
  else {
    btnCancelar.style.display = "none";
    btnPagar.style.display = "none";
  }

}

const p = document.querySelector(".TotalVenta");
function CalcularTotal() {


  let TotalCobrar;
  TotalCobrar = ticketMemoria.reduce((acum, producto) => parseFloat(acum + parseFloat(producto.precioTotal)), 0);
  console.log(TotalCobrar);
  p.textContent = "Total: $" + TotalCobrar;
}

CargarTicketMemoria();
