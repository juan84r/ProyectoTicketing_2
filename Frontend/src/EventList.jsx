import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const EventList = () => {
  const [events, setEvents] = useState([]);
  const [myReservations, setMyReservations] = useState([]);
  const [showRes, setShowRes] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    fetch("http://localhost:5171/api/v1/events") 
      .then(res => res.json())
      .then(data => setEvents(data))
      .catch(err => console.error("Error cargando eventos:", err));
  }, []);

  const fetchMyReservations = async () => {
     
    // localStorage.setItem("userId", user.id);
    const userId = localStorage.getItem("userId");
    
    if (!userId) {
        alert("Sesión expirada. Por favor, volvé a loguearte.");
        navigate("/");
        return;
    }

    try {
      const response = await fetch(`http://localhost:5171/api/v1/reservations/user/${userId}`);
      if (!response.ok) throw new Error("Error en el servidor");
      
      const data = await response.json();
      setMyReservations(data);
      setShowRes(!showRes);
    } catch (error) {
      console.error("Error:", error);
      alert("No se pudieron cargar las reservas.");
    }
  };

  const handleLogout = () => {
    localStorage.clear();
    navigate("/");
  };

  return (
    <div style={{ padding: '40px', backgroundColor: '#1a1a1a', minHeight: '100vh', color: 'white', textAlign: 'center', position: 'relative' }}>
      
      {/* Botón Cerrar Sesión arriba a la derecha */}
      <button onClick={handleLogout} style={logoutBtnStyle}>Cerrar Sesión</button>
      
      <h1 style={{ marginBottom: '40px' }}>🎸 Cartelera de Eventos</h1>
      
      {/* 1. GRILLA DE EVENTOS */}
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

      {/* ESPACIO DIVISOR */}
      <div style={{ margin: '60px 0' }}>
          <hr style={{ borderColor: '#333' }} />
      </div>

      {/* 2. BOTÓN DE RESERVAS AL FINAL */}
      <div style={{ paddingBottom: '100px' }}>
          <button onClick={fetchMyReservations} style={resBtnStyle}>
            {showRes ? 'Ocultar Mis Compras' : '🎫 Ver Mis Reservas'}
          </button>

          {showRes && (
            <div style={resContainerStyle}>
              <h2 style={{ color: '#f1c40f', marginBottom: '20px' }}>Mis Tickets</h2>
              {myReservations.length > 0 ? (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '10px', alignItems: 'center' }}>
                  {myReservations.map(res => (
                    <div key={res.id} style={ticketStyle}>
                      <div style={{ textAlign: 'left' }}>
                        <h4 style={{ margin: 0, color: '#2ecc71' }}>{res.eventName}</h4>
                        <p style={{ margin: '5px 0' }}>Sector: {res.sectorName} | Asiento: <strong>{res.seatNumber}</strong></p>
                        <small style={{ color: '#95a5a6' }}>Comprado: {new Date(res.reservedAt).toLocaleString()}</small>
                      </div>
                    </div>
                  ))}
                </div>
              ) : (
                <p>Todavía no tenés ninguna reserva.</p>
              )}
            </div>
          )}
      </div>
    </div>
  );
};

// --- ESTILOS ---
const cardStyle = { backgroundColor: '#2c3e50', padding: '20px', borderRadius: '15px', width: '280px', textAlign: 'center' };
const btnStyle = { marginTop: '15px', padding: '10px', backgroundColor: '#2ecc71', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer', fontWeight: 'bold', width: '100%' };
const logoutBtnStyle = { position: 'absolute', top: '20px', right: '20px', padding: '10px', backgroundColor: '#e74c3c', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer' };
const resBtnStyle = { padding: '15px 30px', backgroundColor: '#f1c40f', color: '#1a1a1a', border: 'none', borderRadius: '30px', cursor: 'pointer', fontWeight: 'bold', fontSize: '1.1rem' };
const resContainerStyle = { marginTop: '30px', backgroundColor: '#262626', padding: '30px', borderRadius: '20px' };
const ticketStyle = { backgroundColor: '#2c3e50', padding: '15px', borderRadius: '10px', width: '100%', maxWidth: '500px', borderLeft: '5px solid #f1c40f' };

export default EventList;