import { useState } from "react";
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { logout } from "../auth/authService";
import {
  canManageTypeServices,
  canViewAuditLogs,
  canManagePetServices,
} from "../auth/roleUtil";
import "./MainLayout.css";

export default function MainLayout() {
  const navigate = useNavigate();
  const [collapsed, setCollapsed] = useState(false);

  const handleLogout = async () => {
    try {
      await logout();
    } finally {
      navigate("/");
    }
  };

  return (
    <div className={`main-layout ${collapsed ? "collapsed" : ""}`}>
      <aside className="sidebar">
        <div className="sidebar-top">
          <button
            className="toggle-button"
            onClick={() => setCollapsed(!collapsed)}
          >
            ☰
          </button>
        </div>

        <div className="sidebar-brand">
          <h2>VetSystem</h2>
          <span>{localStorage.getItem("userRole")}</span>
        </div>

      <nav className="sidebar-nav">
        <NavLink to="/dashboard">Dashboard</NavLink>

        <NavLink to="/clients/new">Nuevo cliente</NavLink>

        <NavLink to="/pets">Mascotas</NavLink>

        {canManageTypeServices() && (
          <NavLink to="/type-services">Tipos de servicio</NavLink>
        )}

        {canManagePetServices() && (
          <NavLink to="/pet-services">Servicios</NavLink>
        )}

        {canViewAuditLogs() && (
          <NavLink to="/audit-logs">Auditoría</NavLink>
        )}
      </nav>

        <button className="sidebar-logout" onClick={handleLogout}>
          Cerrar sesión
        </button>
      </aside>

      <main className="main-content">
        <Outlet />
      </main>
    </div>
  );
}