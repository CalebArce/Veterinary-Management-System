import { createBrowserRouter } from "react-router-dom";

import LoginPage from "../pages/LoginPage";
import DashboardPage from "../pages/Dashboard/DashboardPage";
import ClientFormPage from "../pages/Client/ClientFormPage";
import PetsPage from "../pages/Pet/PetsPage";
import PetFormPage from "../pages/Pet/PetFormPage";
import TypeServicesPage from "../pages/TypeService/TypeServicesPage";
import TypeServiceFormPage from "../pages/TypeService/TypeServiceFormPage";
import PetServicesPage from "../pages/PetService/PetServicesPage";
import PetServiceFormPage from "../pages/PetService/PetServiceFormPage";
import AuditLogsPage from "../pages/Audit/AuditLogsPage";

import PrivateRoute from "./PrivateRoute";
import MainLayout from "../layouts/MainLayout";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <LoginPage />,
  },
  {
    element: (
      <PrivateRoute>
        <MainLayout />
      </PrivateRoute>
    ),
    children: [
      {
        path: "/dashboard",
        element: <DashboardPage />,
      },
      {
        path: "/clients/new",
        element: <ClientFormPage />,
      },
      {
        path: "/clients/:id/edit",
        element: <ClientFormPage />,
      },
      {
        path: "/pets",
        element: <PetsPage />,
      },
      {
        path: "/pets/new",
        element: <PetFormPage />,
      },
      {
        path: "/pets/:id/edit",
        element: <PetFormPage />,
      },
      {
        path: "/type-services",
        element: <TypeServicesPage />,
      },
      {
        path: "/type-services/new",
        element: <TypeServiceFormPage />,
      },
      {
        path: "/type-services/:id/edit",
        element: <TypeServiceFormPage />,
      },
      {
        path: "/pet-services",
        element: <PetServicesPage />,
      },
      {
        path: "/pet-services/new",
        element: <PetServiceFormPage />,
      },
    ],
  },
  {
    path: "/audit-logs",
    element: <AuditLogsPage />,
  },
]);