# Instrucciones para Debug de Conexión Frontend-Backend

## Problema: Frontend se queda cargando

### Solución Paso a Paso:

### 1. Verificar que el Backend esté corriendo

**Opción A: Desde PowerShell (manual)**
```powershell
cd C:\Users\Lenovo\OneDrive\Escritorio\sigu_bakend\SIGU.API
dotnet run --launch-profile https
```

**Deberías ver:**
```
Now listening on: https://localhost:7084
Now listening on: http://localhost:5214
```

### 2. Verificar que el Frontend esté corriendo

**Desde otra ventana de PowerShell:**
```powershell
cd C:\Users\Lenovo\OneDrive\Escritorio\sigu_front\sigu-frontend
ng serve --port 4200
```

**Deberías ver:**
```
Compiled successfully!
Local: http://localhost:4200
```

### 3. Verificar la configuración

✅ **Backend configurado:**
- Puerto HTTPS: 7084
- Puerto HTTP: 5214
- CORS habilitado para puerto 4200
- Endpoints bajo `/api`

✅ **Frontend configurado:**
- Puerto: 4200
- `environment.ts` apunta a `https://localhost:7084/api`

### 4. Probar la conexión manualmente

**Desde PowerShell (en el navegador puede dar error de certificado):**
```powershell
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
Invoke-WebRequest -Uri "https://localhost:7084/api/programas" -UseBasicParsing
```

**O desde el navegador:**
- Abre: `https://localhost:7084/api/programas`
- Acepta el certificado de desarrollo (puede mostrar advertencia)

### 5. Problemas comunes y soluciones

**Problema: "No se puede conectar"**
- ✅ Verifica que el backend esté corriendo
- ✅ Verifica que no haya errores en la consola del backend
- ✅ Verifica que el puerto 7084 no esté ocupado por otro proceso

**Problema: "CORS error" en el navegador**
- ✅ Verifica que el puerto 4200 esté en la lista de CORS en `Program.cs`
- ✅ Verifica que el frontend esté en el puerto 4200

**Problema: "Certificado SSL inválido"**
- ✅ Es normal en desarrollo, acepta el certificado
- ✅ El proxy de Angular está configurado para ignorar certificados (`secure: false`)

**Problema: Frontend se queda cargando**
- ✅ Abre la consola del navegador (F12) y revisa los errores
- ✅ Verifica la pestaña Network para ver qué petición está fallando
- ✅ Verifica que el backend responda correctamente

### 6. Verificar desde el navegador

1. Abre `http://localhost:4200` en el navegador
2. Abre las herramientas de desarrollador (F12)
3. Ve a la pestaña **Network**
4. Recarga la página
5. Busca las peticiones a `/api/materias` o `/api/programas`
6. Revisa:
   - **Status**: Debe ser 200 (OK)
   - **URL**: Debe ser `https://localhost:7084/api/...`
   - **CORS**: Si hay error de CORS, verifica la configuración

### 7. Prueba rápida de endpoints

**Backend debe responder a:**
- ✅ `https://localhost:7084/api/programas` - GET
- ✅ `https://localhost:7084/api/materias` - GET
- ✅ `https://localhost:7084/api/usuarios` - GET
- ✅ `https://localhost:7084/api/matriculas` - GET

### 8. Si el backend no inicia

**Verifica errores comunes:**
```powershell
# Verificar si hay errores de compilación
cd C:\Users\Lenovo\OneDrive\Escritorio\sigu_bakend
dotnet build SIGU.API/SIGU.API.csproj

# Verificar si el puerto está ocupado
netstat -ano | findstr ":7084"

# Si está ocupado, mata el proceso (reemplaza PID con el número)
# taskkill /PID <PID> /F
```

### 9. Logs útiles

**Backend logs:**
- Busca errores en rojo en la consola de PowerShell donde corre el backend
- Verifica que la base de datos se haya creado correctamente

**Frontend logs:**
- Abre la consola del navegador (F12)
- Busca errores en rojo
- Revisa la pestaña Network para ver las peticiones HTTP

## Estado actual de la configuración:

✅ Backend configurado para puerto 7084
✅ Frontend configurado para puerto 4200
✅ CORS configurado para permitir conexiones desde puerto 4200
✅ URLs de servicios actualizadas a `https://localhost:7084/api`
✅ Endpoints agrupados bajo `/api`

## Próximos pasos:

1. Asegúrate de que AMBOS servicios estén corriendo
2. Abre el navegador en `http://localhost:4200`
3. Abre la consola del navegador (F12) para ver errores
4. Verifica en la pestaña Network qué peticiones están fallando


