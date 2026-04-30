import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  getPetServices,
  updateBillingStatus,
} from "../../services/petServiceAssignmentService";
import { exportToExcel } from "../../utils/exportToExcel";
import {
  canChangeBillingStatus,
  canExportReports,
  canManagePetServices,
} from "../../auth/roleUtils";
import "./PetServicesPage.css";

const billingStatusLabels = {
  1: "Pendiente",
  2: "Facturado",
  3: "Pagado",
  4: "Cancelado",
};

export default function PetServicesPage() {
  const navigate = useNavigate();

  const [petServices, setPetServices] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadPetServices = async () => {
      try {
        setLoading(true);
        setError("");

        const data = await getPetServices();
        setPetServices(data);
      } catch (err) {
        setError("No se pudieron cargar los servicios asignados");
        console.error("Error cargando servicios asignados:", err);
      } finally {
        setLoading(false);
      }
    };

    loadPetServices();
  }, []);

  const refreshPetServices = async () => {
    const data = await getPetServices();
    setPetServices(data);
  };

  const handleBillingChange = async (id, value) => {
    try {
      await updateBillingStatus(id, Number(value));
      await refreshPetServices();
    } catch (err) {
      alert(
        err.response?.data?.message ||
          "No se pudo actualizar el estado de facturación"
      );
    }
  };

  const handleExportServices = () => {
    const data = petServices.map((service) => ({
      Mascota: service.petName,
      Servicio: service.typeServiceName,
      Fecha: new Date(service.serviceDate).toLocaleString(),
      PrecioFinal: service.finalPrice,
      DuracionMinutos: service.durationMinutes,
      EstadoFacturacion: billingStatusLabels[service.billingStatus],
      Notas: service.notes || "Sin notas",
    }));

    exportToExcel(data, "servicios_asignados", "Servicios");
  };

  return (
    <main className="pet-services-page">
      <header className="pet-services-header">
        <div>
          <h1 className="pet-services-title">Servicios asignados</h1>
          <p className="pet-services-subtitle">
            Control de servicios aplicados a mascotas y estado de facturación.
          </p>
        </div>

        <div className="pet-services-actions">
          <button
            className="secondary-button"
            onClick={() => navigate("/dashboard")}
          >
            Volver
          </button>

          {canExportReports() && (
            <button className="secondary-button" onClick={handleExportServices}>
              Exportar Excel
            </button>
          )}

          {canManagePetServices() && (
            <button
              className="new-pet-service-button"
              onClick={() => navigate("/pet-services/new")}
            >
              + Asignar servicio
            </button>
          )}
        </div>
      </header>

      <section className="pet-services-card">
        {error && <div className="pet-services-error">{error}</div>}

        {loading ? (
          <div className="empty-message">Cargando servicios...</div>
        ) : (
          <div className="table-wrapper">
            <table className="pet-services-table">
              <thead>
                <tr>
                  <th>Mascota</th>
                  <th>Servicio</th>
                  <th>Fecha</th>
                  <th>Precio final</th>
                  <th>Duración</th>
                  <th>Estado facturación</th>
                  <th>Notas</th>
                </tr>
              </thead>

              <tbody>
                {petServices.length === 0 ? (
                  <tr>
                    <td colSpan="7" className="empty-message">
                      No hay servicios asignados.
                    </td>
                  </tr>
                ) : (
                  petServices.map((service) => (
                    <tr key={service.id}>
                      <td>{service.petName}</td>
                      <td>{service.typeServiceName}</td>
                      <td>
                        {new Date(service.serviceDate).toLocaleDateString()}
                      </td>
                      <td>₡{Number(service.finalPrice).toLocaleString()}</td>
                      <td>{service.durationMinutes} min</td>
                      <td>
                        {canChangeBillingStatus() ? (
                          <select
                            className="billing-select"
                            value={service.billingStatus}
                            onChange={(e) =>
                              handleBillingChange(service.id, e.target.value)
                            }
                          >
                            <option value={1}>Pendiente</option>
                            <option value={2}>Facturado</option>
                            <option value={3}>Pagado</option>
                            <option value={4}>Cancelado</option>
                          </select>
                        ) : (
                          <span className="read-only-status">Sin permiso</span>
                        )}
                      </td>
                      <td>{service.notes || "Sin notas"}</td>
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