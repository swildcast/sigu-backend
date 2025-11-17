# Script de prueba de conexión Frontend-Backend
# Ejecutar después de que ambos servicios estén corriendo

Write-Host "=== PRUEBA DE CONEXIÓN FRONTEND-BACKEND ===" -ForegroundColor Cyan
Write-Host ""

# Configurar certificado SSL
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

$baseUrl = "https://localhost:7084/api"

# 1. Verificar que el backend responde
Write-Host "1. Verificando conexión con el backend..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/programas" -UseBasicParsing
    Write-Host "   ✅ Backend conectado - Status: $($response.StatusCode)" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Error conectando al backend: $_" -ForegroundColor Red
    exit 1
}

# 2. Obtener lista de materias
Write-Host "`n2. Obteniendo lista de materias..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/materias" -UseBasicParsing
    $materias = $response.Content | ConvertFrom-Json
    Write-Host "   ✅ Materias obtenidas: $($materias.Count)" -ForegroundColor Green
    if ($materias.Count -gt 0) {
        $materias | Format-Table id, codigo, nombre, creditos -AutoSize
    }
} catch {
    Write-Host "   ❌ Error obteniendo materias: $_" -ForegroundColor Red
}

# 3. Crear una nueva materia
Write-Host "`n3. Creando nueva materia..." -ForegroundColor Yellow
$nuevaMateria = @{
    codigo = "TEST001"
    nombre = "Materia de Prueba"
    creditos = 3
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/materias" -Method POST -Body $nuevaMateria -ContentType "application/json" -UseBasicParsing
    $materiaCreada = $response.Content | ConvertFrom-Json
    $materiaId = $materiaCreada.id
    Write-Host "   ✅ Materia creada - ID: $materiaId, Código: $($materiaCreada.codigo)" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Error creando materia: $_" -ForegroundColor Red
    exit 1
}

# 4. Actualizar la materia
Write-Host "`n4. Actualizando materia (ID: $materiaId)..." -ForegroundColor Yellow
$materiaActualizada = @{
    id = $materiaId
    codigo = "TEST001"
    nombre = "Materia de Prueba - ACTUALIZADA"
    creditos = 4
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/materias/$materiaId" -Method PUT -Body $materiaActualizada -ContentType "application/json" -UseBasicParsing
    $materia = $response.Content | ConvertFrom-Json
    Write-Host "   ✅ Materia actualizada - Nombre: $($materia.nombre), Créditos: $($materia.creditos)" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Error actualizando materia: $_" -ForegroundColor Red
}

# 5. Eliminar la materia
Write-Host "`n5. Eliminando materia (ID: $materiaId)..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/materias/$materiaId" -Method DELETE -UseBasicParsing
    Write-Host "   ✅ Materia eliminada - Status: $($response.StatusCode)" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Error eliminando materia: $_" -ForegroundColor Red
}

# 6. Verificar lista final
Write-Host "`n6. Verificando lista final de materias..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/materias" -UseBasicParsing
    $materias = $response.Content | ConvertFrom-Json
    Write-Host "   ✅ Materias restantes: $($materias.Count)" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Error obteniendo lista final: $_" -ForegroundColor Red
}

Write-Host "`n=== PRUEBA COMPLETA ===" -ForegroundColor Cyan
Write-Host "Ahora puedes probar desde el frontend en http://localhost:4200" -ForegroundColor Green


