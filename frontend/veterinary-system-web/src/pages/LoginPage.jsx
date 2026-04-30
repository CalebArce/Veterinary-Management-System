import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../auth/authService";

export default function LoginPage() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("admin@vet.com");
  const [password, setPassword] = useState("Admin123*");
  const [error, setError] = useState("");

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");

    try {
      await login(email, password);
      navigate("/dashboard");
    } catch (err) {
      setError(
        err.response?.data?.message || "No se pudo iniciar sesión"
      );
    }
  };

  return (
    <main style={styles.page}>
      <form onSubmit={handleSubmit} style={styles.card}>
        <h1 style={styles.title}>Veterinary System</h1>
        <p style={styles.subtitle}>Iniciar sesión</p>

        {error && <div style={styles.error}>{error}</div>}

        <label style={styles.label}>Correo</label>
        <input
          style={styles.input}
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />

        <label style={styles.label}>Contraseña</label>
        <input
          style={styles.input}
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <button style={styles.button} type="submit">
          Entrar
        </button>
      </form>
    </main>
  );
}

const styles = {
  page: {
    minHeight: "100vh",
    display: "grid",
    placeItems: "center",
    background: "#f3f4f6",
    fontFamily: "Arial, sans-serif",
  },
  card: {
    width: "360px",
    background: "#fff",
    padding: "32px",
    borderRadius: "18px",
    boxShadow: "0 20px 40px rgba(0,0,0,0.08)",
  },
  title: {
    margin: 0,
    fontSize: "28px",
  },
  subtitle: {
    color: "#6b7280",
    marginBottom: "24px",
  },
  label: {
    display: "block",
    marginTop: "14px",
    marginBottom: "6px",
    fontWeight: "600",
  },
  input: {
    width: "100%",
    padding: "12px",
    borderRadius: "10px",
    border: "1px solid #d1d5db",
  },
  button: {
    width: "100%",
    marginTop: "24px",
    padding: "12px",
    border: "none",
    borderRadius: "10px",
    background: "#111827",
    color: "#fff",
    fontWeight: "700",
    cursor: "pointer",
  },
  error: {
    background: "#fee2e2",
    color: "#991b1b",
    padding: "10px",
    borderRadius: "10px",
    marginBottom: "16px",
  },
};