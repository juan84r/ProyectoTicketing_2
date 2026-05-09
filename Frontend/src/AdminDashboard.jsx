import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const AdminDashboard = () => {
  const navigate = useNavigate();
  const [eventData, setEventData] = useState({
    name: '',
    numSectors: 1,      // Cantidad de sectores (A, B, C...)
    rowsPerSector: 5,   // Filas por cada sector
    seatsPerRow: 10,    // Asientos en cada fila
    price: 5000,
    venue: 'Estadio Central',
    eventDate: new Date().toISOString()
  });

  const handleLogout = () => {
    localStorage.clear();
    navigate("/");
  };

  const handleCreateFullEvent = async () => {
    if (!eventData.name.trim()) return alert("El nombre es obligatorio");
    
    try {
      // IMPORTANTE: Esta URL debe ser la que dispara la lógica de creación de TODO
      const response = await fetch("http://localhost:5171/api/v1/admin/events/generate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(eventData)
      });

      if (response.ok) {
        const result = await response.json();
        alert(`¡Éxito! Se creó el evento "${eventData.name}" con sus sectores y asientos.`);
        console.log("Evento creado:", result);
      } else {
        const errorText = await response.text();
        alert("Error al generar: " + errorText);
      }
    } catch (error) {
      console.error(error);
      alert("Error de conexión con el servidor");
    }
  };

  return (
    <div style={containerStyle}>
      <button type="button" onClick={handleLogout} style={logoutBtnStyle}>Cerrar Sesión</button>
      
      <div style={cardStyle}>
        <h2 style={{ color: '#2ecc71', marginBottom: '10px' }}>🛠️ Generador de Eventos</h2>
        <p style={{ color: '#bdc3c7', fontSize: '14px', marginBottom: '20px' }}>
          Este proceso creará automáticamente los sectores (Letras) y todos sus asientos.
        </p>
        
        <div style={inputGroup}>
          <label>Nombre del Concierto:</label>
          <input type="text" placeholder="Ej: Rock Festival" value={eventData.name}
            onChange={(e) => setEventData({...eventData, name: e.target.value})} style={inputStyle} />
        </div>

        <div style={{ display: 'flex', gap: '15px', marginTop: '15px' }}>
          <div style={{ flex: 1 }}>
            <label>Cant. Sectores:</label>
            <input type="number" min="1" max="26" value={eventData.numSectors}
              onChange={(e) => setEventData({...eventData, numSectors: parseInt(e.target.value)})} style={inputStyle} />
          </div>
          <div style={{ flex: 1 }}>
            <label>Precio Base:</label>
            <input type="number" value={eventData.price}
              onChange={(e) => setEventData({...eventData, price: parseFloat(e.target.value)})} style={inputStyle} />
          </div>
        </div>

        <div style={{ display: 'flex', gap: '15px', marginTop: '15px' }}>
          <div style={{ flex: 1 }}>
            <label>Filas x Sector:</label>
            <input type="number" value={eventData.rowsPerSector}
              onChange={(e) => setEventData({...eventData, rowsPerSector: parseInt(e.target.value)})} style={inputStyle} />
          </div>
          <div style={{ flex: 1 }}>
            <label>Asientos x Fila:</label>
            <input type="number" value={eventData.seatsPerRow}
              onChange={(e) => setEventData({...eventData, seatsPerRow: parseInt(e.target.value)})} style={inputStyle} />
          </div>
        </div>

        <div style={summaryStyle}>
          Se generarán: <strong>{eventData.numSectors * eventData.rowsPerSector * eventData.seatsPerRow}</strong> asientos en total.
        </div>

        <button onClick={handleCreateFullEvent} style={generateBtnStyle}>
          🚀 GENERAR TODO EL EVENTO
        </button>
      </div>
    </div>
  );
};

// Estilos
const containerStyle = { padding: '50px', backgroundColor: '#121212', minHeight: '100vh', display: 'flex', justifyContent: 'center', color: 'white' };
const cardStyle = { backgroundColor: '#1e1e1e', padding: '30px', borderRadius: '15px', width: '100%', maxWidth: '500px', height: 'fit-content' };
const inputGroup = { display: 'flex', flexDirection: 'column', gap: '5px' };
const inputStyle = { padding: '12px', backgroundColor: '#333', border: '1px solid #444', borderRadius: '8px', color: 'white', width: '100%', marginTop: '5px' };
const summaryStyle = { marginTop: '20px', padding: '15px', backgroundColor: '#2c3e50', borderRadius: '8px', textAlign: 'center', border: '1px dashed #3498db' };
const generateBtnStyle = { marginTop: '20px', width: '100%', padding: '15px', backgroundColor: '#2ecc71', color: 'white', border: 'none', borderRadius: '8px', fontWeight: 'bold', cursor: 'pointer', fontSize: '16px' };
const logoutBtnStyle = { position: 'absolute', top: '20px', right: '20px', padding: '10px 20px', backgroundColor: '#e74c3c', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer' };

export default AdminDashboard;