const Tabla = document.querySelector("table");
const tbody = Tabla.querySelector("tbody");
const p = document.querySelector(".TotalVenta");


let TotalCobrar;
let ticketMemoria = [];
let historialVentas = [];

const inputCantidad = document.querySelector("#cantidad");
const padNumerico = document.querySelector(".pad-numerico");
const btnLimpiar = document.querySelector("#limpiarInput");
const btnProcesarVenta = document.querySelector("#procesarVenta");

const modalEfectivo = document.querySelector("#efectivoModal");
const modalTransferecnia = document.querySelector("#transferenciaModal");
const modalTarjeta = document.querySelector("#tarjetaModal");

document.getElementById("pagoEfectivo").addEventListener("click", function () {
  const pMontoTotal = document.getElementById("monto");
  pMontoTotal.textContent = "Total a pagar: $ " + TotalCobrar;
  inputCantidad.value = "";

  modalEfectivo.hidden = false;
  modalEfectivo.parentElement.hidden = false;
  actualizarCambio();
});

document.getElementById("pagoTransferencia").addEventListener("click", function () {
  modalTransferecnia.hidden = false;
  modalTransferecnia.parentElement.hidden = false;
});

document.getElementById("pagoTarjeta").addEventListener("click", function () {
  modalTarjeta.hidden = false;
  modalTarjeta.parentElement.hidden = false;
  pagoTarjeta();
});

modalEfectivo.addEventListener("click", (e) => {
  if (e.target.closest("#cerrarModalEfectivo")) {
    modalEfectivo.hidden = true;
    modalEfectivo.parentElement.hidden = true;
  }

  if (e.target.closest("#procesarVenta")) {
    modalEfectivo.hidden = true;
    modalEfectivo.parentElement.hidden = true;
    procesarVenta("Efectivo");
  }

  if (e.target.closest("#limpiarInput")) {
    inputCantidad.value = "";
    btnLimpiar.style.display = "none";
    actualizarCambio();
  }
});

modalTransferecnia.addEventListener("click", (e) => {
  if (e.target.closest("#cerrarModalTransferencia")) {
    modalTransferecnia.hidden = true;
    modalTransferecnia.parentElement.hidden = true;
  }

  if (e.target.closest("#procesarTransferencia")) {
    procesarVenta("Transferencia");
    modalTransferecnia.hidden = true;
    modalTransferecnia.parentElement.hidden = true;
  }
});

modalTarjeta.addEventListener("click", (e) => {
  if (e.target.closest("#cerrarModalTarjeta")) {
    modalTarjeta.hidden = true;
    modalTarjeta.parentElement.hidden = true;
  }
});

padNumerico.addEventListener("click", (e) => {
  if (e.target.tagName === "BUTTON") {
    const valor = e.target.textContent;
    inputCantidad.value += valor;
    actualizarCambio();

    if (inputCantidad.value.length > 0) {
      btnLimpiar.style.display = "inline";
    }
  }
});

function pagoTarjeta() {
  const mensajeTarjeta = document.querySelector("#mensajeTarjeta");
  let segundos = 10;
  mensajeTarjeta.textContent = "Leyendo tarjeta...";

  timer = setInterval(() => {
    segundos--;

    if (segundos <= 8 && segundos > 4) {
      mensajeTarjeta.textContent = "Procesando cobro...";
    } else if (segundos <= 4 && segundos > 1) {
      mensajeTarjeta.textContent = "Aprobando transacción...";
    } else if (segundos === 1) {
      mensajeTarjeta.textContent = "Transacción aprobada";
    }

    if (segundos <= 0) {
      modalTarjeta.hidden = true;
      modalTarjeta.parentElement.hidden = true;
      procesarVenta("Tarjeta");
      clearTimeout(timer);
    }
  }, 1000);
}

function actualizarCambio() {
  let cantidad = parseFloat(inputCantidad.value) || 0;
  const errorModal = document.querySelector("#error");
  const cambio = document.querySelector("#Cambio");

  if (cantidad < TotalCobrar) {
    errorModal.textContent = "Monto insuficiente";
    cambio.textContent = "Cambio: $0";
    btnProcesarVenta.disabled = true;
  } else {
    errorModal.textContent = "";
    btnProcesarVenta.disabled = false;
    cambio.textContent = "Cambio: $" + (cantidad - TotalCobrar);
  }
}

function procesarVenta(metodoPago, montoTotal) {
  localStorage.removeItem("ticket");

  const checkbox = document.getElementById("guardarVenta");
  if (checkbox.checked) {
    const fechaActual = new Date();
    const dia = String(fechaActual.getDate()).padStart(2, "0");
    const mes = String(fechaActual.getMonth() + 1).padStart(2, "0");
    const año = fechaActual.getFullYear();
    const fechaFormateada = `${dia} - ${mes} - ${año}`;

    let venta = {
      fecha: fechaFormateada,
      productos: ticketMemoria,
      medioPago: metodoPago,
      monto: TotalCobrar,
    };

    let historialVentas = JSON.parse(localStorage.getItem("historial")) || [];
    historialVentas.push(venta);
    console.log(venta);

    localStorage.setItem("historial", JSON.stringify(historialVentas));

  }

  ticketMemoria = [];
  window.location.href = "index.html";
}

function CargarTicketMemoria() {
  ticketMemoria = JSON.parse(localStorage.getItem("ticket")) || [];
  if (ticketMemoria.length > 0) {
    ticketMemoria.forEach((producto) => {
      const fila = document.createElement("tr");

      const tdNombre = document.createElement("td");
      const tdCantidad = document.createElement("td");
      const tdPUnit = document.createElement("td");
      const tdPTotal = document.createElement("td");

      tdNombre.textContent = producto.nombre;
      tdCantidad.textContent = producto.cantidad;
      tdPUnit.textContent = producto.precioUnitario;
      tdPTotal.textContent = producto.precioTotal;

      fila.appendChild(tdNombre);
      fila.appendChild(tdCantidad);
      fila.appendChild(tdPUnit);
      fila.appendChild(tdPTotal);
      tbody.appendChild(fila);

      CalcularTotal();
    });
  }
}


function CalcularTotal() {
  TotalCobrar = ticketMemoria.reduce(
    (acum, producto) => parseFloat(acum + parseFloat(producto.precioTotal)),
    0
  );
  console.log(TotalCobrar);
  p.textContent = "Total: $" + TotalCobrar;
}

CargarTicketMemoria();

