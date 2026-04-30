import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  createTypeService,
  getTypeServiceById,
  updateTypeService,
} from "../../services/typeServiceService";
import "./TypeServiceFormPage.css";

export default function TypeServiceFormPage() {
  const navigate = useNavigate();
  const { id } = useParams();

  const isEditing = Boolean(id);

  const [form, setForm] = useState({
    name: "",
    description: "",
    price: "",
    durationMinutes: "",
    isActive: true,
  });

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const loadTypeService = async () => {
      if (!isEditing) return;

      try {
        setLoading(true);
        setError("");

        const service = await getTypeServiceById(id);

        setForm({
          name: service.name,
          description: service.description,
          price: service.price,
          durationMinutes: service.durationMinutes,
          isActive: service.isActive,
        });
      } catch (err) {
        setError("No se pudo cargar el tipo de servicio");
        console.error("Error cargando tipo de servicio:", err);
      } finally {
        setLoading(false);
      }
    };

    loadTypeService();
  }, [id, isEditing]);

  const handleChange = (event) => {
    const { name, value, type, checked } = event.target;

    setForm({
      ...form,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const buildPayload = () => {
    const payload = {
      name: form.name,
      description: form.description,
      price: Number(form.price),
      durationMinutes: Number(form.durationMinutes),
    };

    if (isEditing) {
      payload.isActive = form.isActive;
    }

    return payload;
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");

    try {
      setLoading(true);

      const payload = buildPayload();

      if (isEditing) {
        await updateTypeService(id, payload);
      } else {
        await createTypeService(payload);
      }

      navigate("/type-services");
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo guardar el tipo de servicio"
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="type-service-form-page">
      <section className="type-service-form-card">
        <h1>{isEditing ? "Editar servicio" : "Nuevo servicio"}</h1>
        <p className="type-service-form-subtitle">
          Defina el precio, duración y disponibilidad del servicio veterinario.
        </p>

        {error && <div className="form-error">{error}</div>}

        {loading ? (
          <div className="empty-message">Cargando...</div>
        ) : (
          <form onSubmit={handleSubmit} className="type-service-form">
            <label>Nombre</label>
            <input name="name" value={form.name} onChange={handleChange} />

            <label>Descripción</label>
            <textarea
              name="description"
              value={form.description}
              onChange={handleChange}
              rows="4"
            />

            <label>Precio</label>
            <input
              name="price"
              type="number"
              min="0"
              step="0.01"
              value={form.price}
              onChange={handleChange}
            />

            <label>Duración en minutos</label>
            <input
              name="durationMinutes"
              type="number"
              min="1"
              value={form.durationMinutes}
              onChange={handleChange}
            />

            {isEditing && (
              <label className="checkbox-row">
                <input
                  type="checkbox"
                  name="isActive"
                  checked={form.isActive}
                  onChange={handleChange}
                />
                Servicio activo
              </label>
            )}

            <div className="form-actions">
              <button type="button" onClick={() => navigate("/type-services")}>
                Cancelar
              </button>

              <button type="submit" disabled={loading}>
                {loading ? "Guardando..." : "Guardar"}
              </button>
            </div>
          </form>
        )}
      </section>
    </main>
  );
}