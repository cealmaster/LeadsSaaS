# LeadsSaas – Plataforma SaaS de Leads (Multi-tenant, .NET + SQL Server)

Este proyecto es una **API REST multi-tenant** en .NET que centraliza y analiza leads de múltiples fuentes (Ads, formularios, CSV, CRMs).  
Incluye:

- Multi-tenancy con **bases de datos por tenant** (SQL Server).
- Autenticación con **JWT**.
- Endpoints de:
  - Autenticación (`/auth/login`)
  - Leads (`/tenants/{tenantId}/leads`)
  - Analytics (`/tenants/{tenantId}/analytics/leads/summary`)
  - Integraciones (`/tenants/{tenantId}/integrations/{source}/leads`)
  - Ejemplo de consumo de API externa (`/external/ip-info`)
- Documentación interactiva con **Swagger**.

---

## Arquitectura general

- **LeadsSaas.Api**  
  Proyecto ASP.NET Core Web API (.NET 9) que expone endpoints REST.

- **Control Plane (conceptual)**  
  Gestiona tenants, usuarios, roles y datos de conexión a las bases por tenant.

- **Tenant DB (SQL Server)**  
  Una base por empresa con tablas como:
  - `leads`, `campaigns`
  - `lead_status_history`
  - `audit_log`
  - `integration_events`
  - `imports`, `import_errors`
  - `forms`, `form_submissions`

La resolución del tenant se hace vía:

- Parámetro de ruta `{tenantId}` en `/tenants/{tenantId}/...`
- (Opcional) Header `X-Tenant-Id`
- (Opcional) Claim `"tenant"` en el JWT  

El `TenantMiddleware` obtiene el `tenantId`, busca la connection string (`Tenant_{tenantId}` o `DefaultTenant`) y se la pasa al `TenantDbContext`.

---

## Requisitos

- [.NET SDK 9](https://dotnet.microsoft.com/)
- SQL Server (local o en contenedor Docker)
- Git (opcional, para clonar)

---

## Configuración inicial

1. **Clonar el repositorio**

   ```bash
   git clone <tu-repo-url> LeadsSaas
   cd LeadsSaas
