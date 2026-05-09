import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const EventList = () => {
  const [events, setEvents] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    fetch("http://localhost:5171/api/v1/events") 
      .then(res => res.json())
      .then(data => setEvents(data))
      .catch(err => console.error("Error cargando eventos:", err));
  }, []);

  const handleLogout = () => {
    localStorage.clear();
    navigate("/");
  };

  return (
    <div style={{ padding: '40px', backgroundColor: '#1a1a1a', minHeight: '100vh', color: 'white', textAlign: 'center' }}>
      <button onClick={handleLogout} style={logoutBtnStyle}>Cerrar Sesión</button>
      <h1 style={{ marginBottom: '40px' }}>🎸 Cartelera de Eventos</h1>
      
      <div style={{ display: 'flex', gap: '20px', justifyContent: 'center', flexWrap: 'wrap' }}>
        {events.map(event => (
          <div key={event.id} style={cardStyle}>
            <h2 style={{ color: '#3498db' }}>{event.name}</h2>
            <p>Lugar: {event.venue || 'Estadio Principal'}</p>
            <button onClick={() => navigate(`/reserva/${event.id}`)} style={btnStyle}>
              Comprar Entradas
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

const cardStyle = { backgroundColor: '#2c3e50', padding: '20px', borderRadius: '15px', width: '280px', textAlign: 'center' };
const btnStyle = { marginTop: '15px', padding: '10px', backgroundColor: '#2ecc71', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer', fontWeight: 'bold', width: '100%' };
const logoutBtnStyle = { position: 'absolute', top: '20px', right: '20px', padding: '10px', backgroundColor: '#e74c3c', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer' };

export default EventList;