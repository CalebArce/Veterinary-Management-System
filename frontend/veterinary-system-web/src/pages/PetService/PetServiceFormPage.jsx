import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getPets } from "../../services/petService";
import { getTypeServices } from "../../services/typeServiceService";
import { createPetService } from "../../services/petServiceAssignmentService";
import "./PetServiceFormPage.css";

export default function PetServiceFormPage() {
  const navigate = useNavigate();

  const [pets, setPets] = useState([]);
  const [typeServices, setTypeServices] = useState([]);

  const [form, setForm] = useState({
    petId: "",
    typeServiceId: "",
    serviceDate: "",
    notes: "",
  });

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const loadInitialData = async () => {
      try {
        setLoading(true);
        setError("");

        const petsData = await getPets();
        const typeServicesData = await getTypeServices();

        setPets(petsData.filter((pet) => pet.isActive));
        setTypeServices(typeServicesData.filter((service) => service.isActive));
      } catch (err) {
        setError("No se pudieron cargar mascotas o servicios.");
        console.error("Error cargando datos iniciales:", err);
      } finally {
        setLoading(false);
      }
    };

    loadInitialData();
  }, []);

  const selectedTypeService = useMemo(() => {
    return typeServices.find(
      (service) => service.id === Number(form.typeServiceId)
    );
  }, [typeServices, form.typeServiceId]);

  const selectedPet = useMemo(() => {
    return pets.find((pet) => pet.id === Number(form.petId));
  }, [pets, form.petId]);

  const handleChange = (event) => {
    const { name, value } = event.target;

    setForm({
      ...form,
      [name]: value,
    });
  };

  const buildPayload = () => ({
    petId: Number(form.petId),
    typeServiceId: Number(form.typeServiceId),
    serviceDate: form.serviceDate,
    notes: form.notes,
  });

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");

    try {
      setLoading(true);

      await createPetService(buildPayload());

      navigate("/pet-services");
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo asignar el servicio."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="pet-service-form-page">
      <section className="pet-service-form-card">
        <h1>Asignar servicio</h1>
        <p className="pet-service-form-subtitle">
          Seleccione una mascota y un tipo de servicio disponible.
        </p>

        {error && <div className="form-error">{error}</div>}

        {loading ? (
          <div className="empty-message">Cargando...</div>
        ) : (
          <form onSubmit={handleSubmit} className="pet-service-form">
            <label>Mascota</label>
            <select name="petId" value={form.petId} onChange={handleChange}>
              <option value="">Seleccione una mascota</option>
              {pets.map((pet) => (
                <option key={pet.id} value={pet.id}>
                  {pet.name} - {pet.species} ({pet.clientName})
                </option>
              ))}
            </select>

            <label>Tipo de servicio</label>
            <select
              name="typeServiceId"
              value={form.typeServiceId}
              onChange={handleChange}
            >
              <option value="">Seleccione un servicio</option>
              {typeServices.map((service) => (
                <option key={service.id} value={service.id}>
                  {service.name} - ₡{Number(service.price).toLocaleString()}
                </option>
              ))}
            </select>

            {(selectedPet || selectedTypeService) && (
              <div className="summary-box">
                <h3>Resumen</h3>

                {selectedPet && (
                  <p>
                    <strong>Mascota:</strong> {selectedPet.name} -{" "}
                    {selectedPet.species}
                  </p>
                )}

                {selectedTypeService && (
                  <>
                    <p>
                      <strong>Servicio:</strong> {selectedTypeService.name}
                    </p>
                    <p>
                      <strong>Precio:</strong> ₡
                      {Number(selectedTypeService.price).toLocaleString()}
                    </p>
                    <p>
                      <strong>Duración:</strong>{" "}
                      {selectedTypeService.durationMinutes} minutos
                    </p>
                  </>
                )}
              </div>
            )}

            <label>Fecha del servicio</label>
            <input
              name="serviceDate"
              type="datetime-local"
              value={form.serviceDate}
              onChange={handleChange}
            />

            <label>Notas</label>
            <textarea
              name="notes"
              rows="4"
              value={form.notes}
              onChange={handleChange}
              placeholder="Observaciones del servicio..."
            />

            <div className="form-actions">
              <button type="button" onClick={() => navigate("/pet-services")}>
                Cancelar
              </button>

              <button type="submit" disabled={loading}>
                {loading ? "Guardando..." : "Asignar servicio"}
              </button>
            </div>
          </form>
        )}
      </section>
    </main>
  );
}