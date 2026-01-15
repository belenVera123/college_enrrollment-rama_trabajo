
const urlApi = "/api/v1/StudentScores";
const urlAssessments = "/api/v1/Assessments";
const urlEnrollments = "/api/v1/Enrollments";

document.addEventListener("DOMContentLoaded", () => {
    cargarCombos();
    listarPuntajes();
});

async function cargarCombos() {
    try {
        
        const resA = await fetch(urlAssessments);
        const assessments = await resA.json();
        
        document.getElementById("assessmentId").innerHTML += assessments.map(a => 
            `<option value="${a.Id}">${a.Nombre}</option>`).join('');

    
        const resE = await fetch(urlEnrollments);
        const enrollments = await resE.json();


        const lista = enrollments.Items || enrollments.items || enrollments; 
        
        document.getElementById("enrollmentId").innerHTML += lista.map(e => 
            `<option value="${e.Id}">${e.StudentName} - ${e.CourseName}</option>`).join('');
    } catch (error) {
        console.error("Error cargando selectores:", error);
    }
}

async function listarPuntajes() {
    try {
        const response = await fetch(urlApi);
        const datos = await response.json();
        const cuerpo = document.getElementById("cuerpoTabla");
        cuerpo.innerHTML = "";
        datos.forEach(s => {
            cuerpo.innerHTML += `
                <tr>
                    <td>${s.AssessmentName || 'N/A'}</td>
                    <td>${s.CourseName || 'N/A'}</td>
                    <td>${s.StudentName || 'N/A'}</td>
                    <td><span class="badge bg-primary">${s.PuntajeObtenido}</span></td>
                    <td>
                        <button class="btn btn-danger btn-sm" onclick="eliminar(${s.Id})">Eliminar</button>
                    </td>
                </tr>`;
        });
    } catch (error) {
        console.error("Error listando puntajes:", error);
    }
}

async function guardar() {
    const data = {
        AssessmentId: parseInt(document.getElementById("assessmentId").value),
        EnrollmentId: parseInt(document.getElementById("enrollmentId").value),
        PuntajeObtenido: parseInt(document.getElementById("puntaje").value),
        Observaciones: document.getElementById("observaciones").value
    };

    try {
        const response = await fetch(urlApi, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            alert("¡Guardado correctamente!");
            limpiar();
            listarPuntajes();
        } else {
            const errorData = await response.json();
            console.error("Error del servidor:", errorData);
            alert("Error al guardar. Revisa la consola.");
        }
    } catch (error) {
        console.error("Error en la petición POST:", error);
    }
}

function limpiar() {
    document.getElementById("scoreForm").reset();
}

async function eliminar(id) {
    if(confirm("¿Seguro que quieres eliminar este puntaje?")) {
        try {
            const response = await fetch(`${urlApi}/${id}`, { method: "DELETE" });
            if (response.ok) {
                listarPuntajes();
            }
        } catch (error) {
            console.error("Error al eliminar:", error);
        }
    }
}