import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getClients } from "../../services/clientService";
import { createPet, getPetById, updatePet } from "../../services/petService";
import "./PetFormPage.css";

export default function PetFormPage() {
  const navigate = useNavigate();
  const { id } = useParams();

  const isEditing = Boolean(id);

  const [clients, setClients] = useState([]);
  const [form, setForm] = useState({
    clientId: "",
    name: "",
    species: "",
    breed: "",
    age: "",
    weight: "",
    isActive: true,
  });

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const loadInitialData = async () => {
      try {
        setLoading(true);
        setError("");

        const clientsData = await getClients();
        setClients(clientsData);

        if (isEditing) {
          const pet = await getPetById(id);

          setForm({
            clientId: pet.clientId,
            name: pet.name,
            species: pet.species,
            breed: pet.breed,
            age: pet.age,
            weight: pet.weight,
            isActive: pet.isActive,
          });
        }
      } catch (err) {
        setError("No se pudo cargar la información");
        console.error("Error cargando formulario de mascota:", err);
      } finally {
        setLoading(false);
      }
    };

    loadInitialData();
  }, [id, isEditing]);

  const handleChange = (event) => {
    const { name, value, type, checked } = event.target;

    setForm({
      ...form,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const buildPayload = () => {
    if (isEditing) {
      return {
        name: form.name,
        species: form.species,
        breed: form.breed,
        age: Number(form.age),
        weight: Number(form.weight),
        isActive: form.isActive,
      };
    }

    return {
      clientId: Number(form.clientId),
      name: form.name,
      species: form.species,
      breed: form.breed,
      age: Number(form.age),
      weight: Number(form.weight),
    };
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");

    try {
      setLoading(true);

      const payload = buildPayload();

      if (isEditing) {
        await updatePet(id, payload);
      } else {
        await createPet(payload);
      }

      navigate("/pets");
    } catch (err) {
      setError(err.response?.data?.message || "No se pudo guardar la mascota");
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="pet-form-page">
      <section className="pet-form-card">
        <h1>{isEditing ? "Editar mascota" : "Nueva mascota"}</h1>
        <p className="pet-form-subtitle">
          {isEditing
            ? "Actualice los datos de la mascota seleccionada"
            : "Registre una mascota y asígnela a un cliente existente"}
        </p>

        {error && <div className="form-error">{error}</div>}

        {loading ? (
          <div className="empty-message">Cargando...</div>
        ) : (
          <form onSubmit={handleSubmit} className="pet-form">
            {!isEditing && (
              <>
                <label>Cliente propietario</label>
                <select
                  name="clientId"
                  value={form.clientId}
                  onChange={handleChange}
                >
                  <option value="">Seleccione un cliente</option>
                  {clients.map((client) => (
                    <option key={client.id} value={client.id}>
                      {client.fullName} - {client.identification}
                    </option>
                  ))}
                </select>
              </>
            )}

            <label>Nombre</label>
            <input name="name" value={form.name} onChange={handleChange} />

            <label>Especie</label>
            <input
              name="species"
              value={form.species}
              onChange={handleChange}
              placeholder="Perro, gato, conejo..."
            />

            <label>Raza</label>
            <input name="breed" value={form.breed} onChange={handleChange} />

            <label>Edad</label>
            <input
              name="age"
              type="number"
              min="0"
              value={form.age}
              onChange={handleChange}
            />

            <label>Peso</label>
            <input
              name="weight"
              type="number"
              min="0"
              step="0.01"
              value={form.weight}
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
                Mascota activa
              </label>
            )}

            <div className="form-actions">
              <button type="button" onClick={() => navigate("/pets")}>
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