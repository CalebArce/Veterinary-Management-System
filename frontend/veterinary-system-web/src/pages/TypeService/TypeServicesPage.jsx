import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  deleteTypeService,
  getTypeServices,
} from "../../services/typeServiceService";
import { canManageTypeServices } from "../../auth/roleUtils";
import "./TypeServicesPage.css";

export default function TypeServicesPage() {
  const navigate = useNavigate();

  const [typeServices, setTypeServices] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const refreshTypeServices = async () => {
    const data = await getTypeServices();
    setTypeServices(data);
  };

  useEffect(() => {
    const loadTypeServices = async () => {
      try {
        setLoading(true);
        setError("");

        const data = await getTypeServices();
        setTypeServices(data);
      } catch (err) {
        setError("No se pudieron cargar los tipos de servicio");
        console.error("Error cargando servicios:", err);
      } finally {
        setLoading(false);
      }
    };

    loadTypeServices();
  }, []);

  const handleDelete = async (id) => {
    const confirmDelete = window.confirm(
      "¿Está seguro de que desea eliminar este tipo de servicio?"
    );

    if (!confirmDelete) return;

    try {
      await deleteTypeService(id);
      await refreshTypeServices();
    } catch (err) {
      alert(
        err.response?.data?.message ||
          "No se pudo eliminar el tipo de servicio"
      );
    }
  };

  return (
    <main className="type-services-page">
      <header className="type-services-header">
        <div>
          <h1 className="type-services-title">Tipos de servicio</h1>
          <p className="type-services-subtitle">
            Catálogo de servicios veterinarios disponibles.
          </p>
        </div>

        <div className="type-services-actions">
          <button
            className="secondary-button"
            onClick={() => navigate("/dashboard")}
          >
            Volver
          </button>

          {canManageTypeServices() && (
            <button
              className="new-type-service-button"
              onClick={() => navigate("/type-services/new")}
            >
              + Nuevo servicio
            </button>
          )}
        </div>
      </header>

      <section className="type-services-card">
        {error && <div className="type-services-error">{error}</div>}

        {loading ? (
          <div className="empty-message">Cargando servicios...</div>
        ) : (
          <div className="table-wrapper">
            <table className="type-services-table">
              <thead>
                <tr>
                  <th>Nombre</th>
                  <th>Descripción</th>
                  <th>Precio</th>
                  <th>Duración</th>
                  <th>Estado</th>
                  {canManageTypeServices() && <th>Acciones</th>}
                </tr>
              </thead>

              <tbody>
                {typeServices.length === 0 ? (
                  <tr>
                    <td colSpan="6" className="empty-message">
                      No hay tipos de servicio registrados.
                    </td>
                  </tr>
                ) : (
                  typeServices.map((service) => (
                    <tr key={service.id}>
                      <td>{service.name}</td>
                      <td>{service.description}</td>
                      <td>₡{Number(service.price).toLocaleString()}</td>
                      <td>{service.durationMinutes} min</td>
                      <td>
                        <span
                          className={
                            service.isActive
                              ? "status-badge active"
                              : "status-badge inactive"
                          }
                        >
                          {service.isActive ? "Activo" : "Inactivo"}
                        </span>
                      </td>
                      {canManageTypeServices() && (
                        <td className="actions-cell">
                          <button
                            className="edit-button"
                            onClick={() => navigate(`/type-services/${service.id}/edit`)}
                          >
                            Editar
                          </button>

                          <button
                            className="delete-button"
                            onClick={() => handleDelete(service.id)}
                          >
                            Eliminar
                          </button>
                        </td>
                      )}
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        )}
      </section>
    </main>
  );
}