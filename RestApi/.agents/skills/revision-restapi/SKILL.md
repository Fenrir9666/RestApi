---
name: revision-restapi
description: Revisa el código de la RestApi antes de un commit.
  Úsala cuando pidan revisar, auditar o verificar cambios.
---

# Revisión de código RestApi

1. Ejecuta git diff y revisa solo lo que cambió.
2. Revisa cada punto y marca ✔ o ✘:
   - SQL con parámetros (@nombre), nunca concatenado
   - Los endpoints solo llaman al servicio
   - El SQL solo vive en Datos/
   - Validaciones en el servicio, no en el endpoint
   - Códigos HTTP correctos (201, 204, 400, 404, 409)
   - Cada endpoint nuevo tiene su prueba en el .http
   - Sin contraseñas en el código; .env en .gitignore
3. Por cada ✘ indica archivo, línea, por qué está mal
   y cómo corregirlo.
4. NO corrijas el código: explica el error para que
   el estudiante lo arregle.
5. Termina con "Listo para commit" o
   "Corrige antes de hacer commit".