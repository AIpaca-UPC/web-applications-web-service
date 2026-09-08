# Guía de contribución — Rumbo Web Service

## Flujo de trabajo

1. Actualizar `develop`.
2. Crear `feature/<bounded-context>-<funcionalidad>` o `feature/<funcionalidad>`.
3. Mantener dominio, aplicación, infraestructura e interfaces separados cuando el contexto lo requiera.
4. Agregar o actualizar pruebas junto con la funcionalidad.
5. Usar Conventional Commits.
6. Abrir Pull Request hacia `develop`.

## Ejemplos

- `feat(iam): add authentication endpoint`
- `feat(routes): add trip status query`
- `test(routes): add trip status integration tests`
- `fix(api): correct validation response`
- `refactor(shared): extract base entity`

Los nombres definitivos de bounded contexts deben coincidir con el Project Report.
