const urlAssessments = "/api/v1/Assessments";
const urlCourses = "/api/v1/Courses";

document.addEventListener("DOMContentLoaded", () => {
    cargarCursos();
    listarEvaluaciones();

    
    document.getElementById("assessmentForm").addEventListener("submit", async (e) => {
        e.preventDefault();
        await guardar();
    });
});

async function cargarCursos() {
    try {
        const res = await fetch(urlCourses);
        if (!res.ok) throw new Error("Error: No se pudo obtener la lista de cursos");
        
        const data = await res.json();
        
        
        const listaCursos = data.Items || data.items || []; 

        const select = document.getElementById("courseId");
        
        select.innerHTML = '<option value="">Seleccione un curso...</option>';
        listaCursos.forEach(c => {
            
            select.innerHTML += `<option value="${c.Id}">${c.Nombre}</option>`;
        });
        
        console.log("Cursos cargados:", listaCursos);
    } catch (error) {
        console.error("Error al cargar cursos:", error);
    }
}

async function listarEvaluaciones() {
    try {
        const res = await fetch(urlAssessments);
        const datos = await res.json();
        const cuerpo = document.getElementById("cuerpoTabla");
        cuerpo.innerHTML = "";

        datos.forEach(a => {
           
            cuerpo.innerHTML += `
                <tr>
                    <td>${a.CourseName || 'Sin curso'}</td>
                    <td>${a.Nombre}</td>
                    <td>${a.Puntaje}</td>
                    <td>${new Date(a.FechaEvaluacion).toLocaleDateString()}</td>
                    <td>
                        <button class="btn btn-danger btn-sm" onclick="eliminar(${a.Id})">Eliminar</button>
                    </td>
                </tr>`;
        });
    } catch (error) {
        console.error("Error al listar:", error);
    }
}

async function guardar() {
    const data = {
        
        CourseId: parseInt(document.getElementById("courseId").value),
        Nombre: document.getElementById("nombre").value,
        Puntaje: parseInt(document.getElementById("puntaje").value),
        FechaEvaluacion: document.getElementById("fechaEvaluacion").value,
        Descripcion: document.getElementById("descripcion").value
    };

    const res = await fetch(urlAssessments, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

    if (res.ok) {
        alert("Evaluación guardada con éxito");
        limpiarFormulario();
        listarEvaluaciones();
    } else {
        alert("Error al guardar. Verifica los datos en la consola.");
    }
}

function limpiarFormulario() {
    document.getElementById("assessmentForm").reset();
    document.getElementById("assessmentId").value = "";
}

async function eliminar(id) {
    if (confirm("¿Estás seguro de eliminar esta evaluación?")) {
        await fetch(`${urlAssessments}/${id}`, { method: "DELETE" });
        listarEvaluaciones();
    }
}