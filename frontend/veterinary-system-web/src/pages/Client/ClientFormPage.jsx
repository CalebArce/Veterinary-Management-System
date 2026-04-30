import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  createClient,
  getClientById,
  updateClient,
} from "../../services/clientService";
import "./ClientFormPage.css";

export default function ClientFormPage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const isEditing = Boolean(id);

  const [form, setForm] = useState({
    identification: "",
    fullName: "",
    phone: "",
    email: "",
    address: "",
    isActive: true,
  });

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const loadClient = async () => {
      if (!isEditing) return;

      try {
        const client = await getClientById(id);

        setForm({
          identification: client.identification,
          fullName: client.fullName,
          phone: client.phone,
          email: client.email,
          address: client.address,
          isActive: client.isActive,
        });
      } catch {
        setError("No se pudo cargar el cliente");
      }
    };

    loadClient();
  }, [id, isEditing]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;

    setForm({
      ...form,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      if (isEditing) {
        await updateClient(id, form);
      } else {
        await createClient(form);
      }

      navigate("/dashboard");
    } catch (err) {
      setError(err.response?.data?.message || "No se pudo guardar el cliente");
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="client-form-page">
      <section className="client-form-card">
        <h1>{isEditing ? "Editar cliente" : "Nuevo cliente"}</h1>

        {error && <div className="form-error">{error}</div>}

        <form onSubmit={handleSubmit} className="client-form">
          <label>Identificación</label>
          <input
            name="identification"
            value={form.identification}
            onChange={handleChange}
          />

          <label>Nombre completo</label>
          <input
            name="fullName"
            value={form.fullName}
            onChange={handleChange}
          />

          <label>Teléfono</label>
          <input name="phone" value={form.phone} onChange={handleChange} />

          <label>Correo</label>
          <input
            name="email"
            type="email"
            value={form.email}
            onChange={handleChange}
          />

          <label>Dirección</label>
          <input
            name="address"
            value={form.address}
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
              Cliente activo
            </label>
          )}

          <div className="form-actions">
            <button type="button" onClick={() => navigate("/dashboard")}>
              Cancelar
            </button>

            <button type="submit" disabled={loading}>
              {loading ? "Guardando..." : "Guardar"}
            </button>
          </div>
        </form>
      </section>
    </main>
  );
}