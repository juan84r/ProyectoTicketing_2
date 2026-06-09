import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const AdminDashboard = () => {
  const navigate = useNavigate();
  const [eventData, setEventData] = useState({
    name: '',
    venue: '', 
    numSectors: 2,
    rowsPerSector: 5,
    seatsPerRow: 10,
    price: 5000,
    eventDate: new Date().toISOString()
  });

  const handleLogout = () => {
    localStorage.clear();
    navigate("/");
  };

  const handleCreateFullEvent = async () => {
    if (!eventData.name.trim() || !eventData.venue.trim()) {
        return alert("El nombre y el lugar son obligatorios");
    }
    
    try {
      const response = await fetch("http://localhost:5171/api/v1/admin/events", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(eventData)
      });

      if (response.ok) {
        alert(`¡Éxito! Evento "${eventData.name}" creado en "${eventData.venue}"`);
      } else {
        const errorText = await response.text();
        alert("Error: " + errorText);
      }
    } catch (error) {
      console.error(error);
      alert("Error de conexión");
    }
  };

  return (
    <div style={containerStyle}>
      <button onClick={handleLogout} style={logoutBtnStyle}>Cerrar Sesión</button>
      
      <div style={cardStyle}>
        <h2 style={{ color: '#2ecc71', marginBottom: '20px' }}>🛠️ Generador de Eventos</h2>
        
        {/* INPUT NOMBRE */}
        <div style={inputGroup}>
          <label>Nombre del Evento:</label>
          <input type="text" placeholder="Ej: Rock Festival" value={eventData.name}
            onChange={(e) => setEventData({...eventData, name: e.target.value})} style={inputStyle} />
        </div>

        {/* NUEVO: INPUT LUGAR (VENUE) */}
        <div style={{ ...inputGroup, marginTop: '15px' }}>
          <label>Lugar / Estadio:</label>
          <input type="text" placeholder="Ej: Luna Park" value={eventData.venue}
            onChange={(e) => setEventData({...eventData, venue: e.target.value})} style={inputStyle} />
        </div>

        <div style={{ display: 'flex', gap: '15px', marginTop: '15px' }}>
          <div style={{ flex: 1 }}>
            <label>Cant. Sectores:</label>
            <input type="number" value={eventData.numSectors}
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
          Total de asientos: <strong>{eventData.numSectors * eventData.rowsPerSector * eventData.seatsPerRow}</strong>
        </div>

        <button onClick={handleCreateFullEvent} style={generateBtnStyle}>
          🚀 GENERAR EVENTO COMPLETO
        </button>
      </div>
    </div>
  );
};

// Estilos
const containerStyle = { padding: '50px', backgroundColor: '#121212', minHeight: '100vh', display: 'flex', justifyContent: 'center', color: 'white' };
const cardStyle = { backgroundColor: '#1e1e1e', padding: '30px', borderRadius: '15px', width: '100%', maxWidth: '500px' };
const inputGroup = { display: 'flex', flexDirection: 'column', gap: '5px' };
const inputStyle = { padding: '12px', backgroundColor: '#333', border: '1px solid #444', borderRadius: '8px', color: 'white' };
const summaryStyle = { marginTop: '20px', padding: '15px', backgroundColor: '#2c3e50', borderRadius: '8px', textAlign: 'center' };
const generateBtnStyle = { marginTop: '20px', width: '100%', padding: '15px', backgroundColor: '#2ecc71', color: 'white', border: 'none', borderRadius: '8px', fontWeight: 'bold', cursor: 'pointer' };
const logoutBtnStyle = { position: 'absolute', top: '20px', right: '20px', padding: '10px', backgroundColor: '#e74c3c', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer' };

export default AdminDashboard;