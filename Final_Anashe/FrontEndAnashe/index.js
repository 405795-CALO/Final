async function cargarConfiguraciones() {
    try {
        const response = await fetch("https://localhost:7033/api/configuraciones");
        
        if (!response.ok) {
            throw new Error("Error al obtener las configuraciones");
        }

        // Procesa la respuesta como un array
        const configuraciones = await response.json(); 

        // Aplica las configuraciones al formulario directamente
        const formulario = document.getElementById("sucursalForm");
        configuraciones.forEach(config => {
            formulario.style[config.nombre] = config.valor; // Aplica las configuraciones como estilo CSS
        });

        console.log("Configuraciones aplicadas automáticamente.");
    } catch (error) {
        console.error("Error cargando configuraciones:", error);
    }
}

// Llama esta función al cargar la página
document.addEventListener("DOMContentLoaded", cargarConfiguraciones);

document.addEventListener("DOMContentLoaded", () => {
    const apiBaseUrl = "https://localhost:7033/api"; // Cambia esto a la URL base de tu API.

    // Cargar la sucursal al cargar la página
    fetch(`${apiBaseUrl}/sucursal-not-bsas`)
        .then((response) => {
            if (!response.ok) {
                throw new Error("Error al obtener la sucursal");
            }
            return response.json();
        })
        .then((sucursal) => {
            console.log(sucursal);
            const fechaAlta = new Date(sucursal.fechaAlta);
            console.log(fechaAlta);

            // Suponiendo que `data.fechaAlta` viene en formato ISO: "2024-11-24T13:00:00.000Z"
            const fechaAltaOriginal = sucursal.fechaAlta.split('T'); // Divide la fecha y la hora
            const fecha = fechaAltaOriginal[0]; // Obtén la parte de la fecha (YYYY-MM-DD)
            const hora = fechaAltaOriginal[1].slice(0, 8); // Obtén la hora (HH:mm)

            // Rellenar los inputs con los datos de la sucursal
            document.getElementById("guid").value = sucursal.id; //borrar esto
            document.getElementById("nombre").value = sucursal.nombre;
            document.getElementById("nombreCiudad").value = sucursal.idCiudad;
            document.getElementById("tipo").value = sucursal.idTipo;
            document.getElementById("provincia").value = sucursal.idProvincia;
            document.getElementById("telefono").value = sucursal.telefono;
            document.getElementById("nombreTitular").value = sucursal.nombreTitular;
            document.getElementById("apellidoTitular").value = sucursal.apellidoTitular;
            document.getElementById("fechaAlta").value = `${fecha}T${hora}`; // Asignar fecha y hora al input
            //new Date(sucursal.fechaAlta).toISOString().slice(0,19); // Formato yyyy-mm-dd
        })
        .catch((error) => {
            console.error("Error:", error);
            alert("Hubo un problema al cargar los datos de la sucursal.");
        });
});

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("sucursalForm").addEventListener("submit", async function (event) {
        event.preventDefault(); // Evita el envío del formulario por defecto

        // Obtén los datos del formulario
        const id = document.getElementById("guid").value; // Reemplaza con el ID correspondiente si es fijo
        const sucursal = {
            
            nombre: document.getElementById("nombre").value,
            idCiudad: document.getElementById("nombreCiudad").value,
            idTipo: document.getElementById("tipo").value,
            idProvincia: document.getElementById("provincia").value,
            telefono: document.getElementById("telefono").value,
            nombreTitular: document.getElementById("nombreTitular").value,
            apellidoTitular: document.getElementById("apellidoTitular").value,
            fechaAlta: document.getElementById("fechaAlta").value
        };
        
        console.log(JSON.stringify(sucursal));

        try {
            // Enviar el PUT al backend
            const response = await fetch(`https://localhost:7033/api/sucursal/${id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(sucursal), // Convierte el objeto a JSON
            });

            // Verifica si la operación fue exitosa
            if (response.ok) {
                const result = await response.json();
                if (result.success) {
                    alert("Sucursal actualizada correctamente.");
                } else {
                    alert("Error al actualizar la sucursal: " + result.errorMessage);
                }
            } else {
                alert("Error en la solicitud: " + response.status + "-" + response.statusText);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Ocurrió un error inesperado al intentar actualizar la sucursal.");
        }
    })
});
