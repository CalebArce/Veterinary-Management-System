import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { deletePet, getPets } from "../../services/petService";
import "./PetsPage.css";

export default function PetsPage() {
  const navigate = useNavigate();

  const [pets, setPets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const refreshPets = async () => {
    const data = await getPets();
    setPets(data);
  };

  useEffect(() => {
    const loadPets = async () => {
      try {
        setLoading(true);
        setError("");

        const data = await getPets();
        setPets(data);
      } catch (err) {
        setError("No se pudieron cargar las mascotas");
        console.error("Error cargando mascotas:", err);
      } finally {
        setLoading(false);
      }
    };

    loadPets();
  }, []);

  const handleDelete = async (id) => {
    const confirmDelete = window.confirm(
      "¿Está seguro de que desea eliminar esta mascota?"
    );

    if (!confirmDelete) return;

    try {
      await deletePet(id);
      await refreshPets();
    } catch (err) {
      alert(err.response?.data?.message || "No se pudo eliminar la mascota");
    }
  };

  return (
    <main className="pets-page">
      <header className="pets-header">
        <div>
          <h1 className="pets-title">Mascotas</h1>
          <p className="pets-subtitle">
            Gestión de mascotas registradas y asociadas a clientes.
          </p>
        </div>

        <div className="pets-header-actions">
          <button
            className="secondary-button"
            onClick={() => navigate("/dashboard")}
          >
            Volver
          </button>

          <button
            className="new-pet-button"
            onClick={() => navigate("/pets/new")}
          >
            + Nueva mascota
          </button>
        </div>
      </header>

      <section className="pets-card">
        {error && <div className="pets-error">{error}</div>}

        {loading ? (
          <div className="empty-message">Cargando mascotas...</div>
        ) : (
          <div className="table-wrapper">
            <table className="pets-table">
              <thead>
                <tr>
                  <th>Nombre</th>
                  <th>Especie</th>
                  <th>Raza</th>
                  <th>Edad</th>
                  <th>Peso</th>
                  <th>Cliente</th>
                  <th>Estado</th>
                  <th>Acciones</th>
                </tr>
              </thead>

              <tbody>
                {pets.length === 0 ? (
                  <tr>
                    <td colSpan="8" className="empty-message">
                      No hay mascotas registradas.
                    </td>
                  </tr>
                ) : (
                  pets.map((pet) => (
                    <tr key={pet.id}>
                      <td>{pet.name}</td>
                      <td>{pet.species}</td>
                      <td>{pet.breed}</td>
                      <td>{pet.age}</td>
                      <td>{pet.weight} kg</td>
                      <td>{pet.clientName}</td>
                      <td>
                        <span
                          className={
                            pet.isActive
                              ? "status-badge active"
                              : "status-badge inactive"
                          }
                        >
                          {pet.isActive ? "Activa" : "Inactiva"}
                        </span>
                      </td>
                      <td className="actions-cell">
                        <button
                          className="edit-button"
                          onClick={() => navigate(`/pets/${pet.id}/edit`)}
                        >
                          Editar
                        </button>

                        <button
                          className="delete-button"
                          onClick={() => handleDelete(pet.id)}
                        >
                          Eliminar
                        </button>
                      </td>
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