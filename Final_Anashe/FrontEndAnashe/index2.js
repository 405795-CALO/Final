var configUrl = "https://localhost:7033/api/configuraciones";
var getSucursalUrl = "https://localhost:7033/api/sucursal-not-bsas"
var putSucursalUrl = "https://localhost:7033/api/sucursal/{id}"

function getConfigs() {
fetch("https://localhost:7033/api/configuraciones")
    .then((response) => response.json())
    .then((data) => {
    data.data.forEach((config) => {
        if (config.nombre === "padding-top") {
        document.getElementById("sucursalForm").style.paddingTop =
            config.valor;
        } else if (config.nombre === "padding-left") {
        document.getElementById("sucursalForm").style.paddingLeft =
            config.valor;
        }
    });
    })
    .catch((error) =>
    console.error("Error al obtener configuraciones:", error)
    );
}
function getSucursal() {
fetch(getSucursalUrl)
    .then((response) => response.json())
    .then((sucursal) => {
    document.getElementById("nombre").value = sucursal.nombre;
    document.getElementById("nombreCiudad").value = sucursal.idCiudad;
    document.getElementById("tipo").value = sucursal.idTipo;
    document.getElementById("provincia").value = sucursal.idProvincia;
    document.getElementById("telefono").value = sucursal.telefono;
    document.getElementById("nombreTitular").value = sucursal.nombreTitular;
    document.getElementById("apellidoTitular").value =
        sucursal.apellidoTitular;
    document.getElementById("fechaAlta").value = sucursal.fechaAlta;
    })
    .catch((error) => console.error("Error al obtener sucursal:", error));
}

document.addEventListener("DOMContentLoaded", () => {
getConfigs();
getSucursal();

document
    .getElementById("sucursalForm")
    .addEventListener("submit", function (event) {
    event.preventDefault();

    const sucursalData = {
        nombre: document.getElementById("nombre").value,
        nombreCiudad: document.getElementById("nombreCiudad").value,
        idTipo: document.getElementById("tipo").value,
        idProvincia: document.getElementById("provincia").value,
        telefono: document.getElementById("telefono").value,
        nombreTitular: document.getElementById("nombreTitular").value,
        apellidoTitular: document.getElementById("apellidoTitular").value,
    };

    fetch(putSucursalUrl, {
        method: "PUT",
        headers: {
        "Content-Type": "application/json",
        },
        body: JSON.stringify(sucursalData),
    })
        .then((response) => response.json())
        .then((data) => {
        if (data.success) {
            alert("La sucursal se ha actualizado correctamente.");
        } else {
            alert("Error al actualizar la sucursal.");
        }
        })
        .catch((error) => {
        console.error("Error al actualizar sucursal:", error);
        alert("Error al actualizar la sucursal.");
        });
    });
});
