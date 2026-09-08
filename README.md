# Rumbo — Web Service

RESTful API del producto **Rumbo**, desarrollada por **AIpaca** para el curso **1ASI0730 Aplicaciones Web** (NRC 8137, ciclo 2026-20).

Este repositorio corresponde exclusivamente al **Web Service**. La Landing Page y el Frontend Web Application se mantienen en repositorios separados.

## Propósito

El servicio expondrá los recursos y operaciones requeridos por la aplicación web de Rumbo. La estructura de negocio se organizará a partir de los bounded contexts definidos y validados en el diseño DDD del Project Report.

## Stack objetivo del curso

- ASP.NET Core
- C#
- Entity Framework Core
- RESTful API
- Base de datos relacional según la decisión técnica del equipo

## Estructura base

```text
web-applications-web-service/
├── docs/
├── src/
│   ├── Contexts/
│   └── Shared/
├── tests/
├── .gitignore
├── CONTRIBUTING.md
└── README.md
```

`src/Contexts/` se dividirá por bounded context una vez que el equipo valide el Design-Level EventStorming y la arquitectura DDD. `tests/` contendrá pruebas unitarias y de integración/aceptación conforme avance la implementación.

## GitFlow

- `main`: versión estable.
- `develop`: integración.
- `feature/...`: funcionalidades o bounded contexts.

No se debe desarrollar directamente sobre `main`.

## Repositorios relacionados

- Project Report: https://github.com/AIpaca-UPC/web-applications-project-report
- Landing Page: https://github.com/AIpaca-UPC/landing-page
- Web Application: https://github.com/AIpaca-UPC/web-applications-web-app
