import { useEffect, useState } from 'react'
import './App.css'
import Login from "./Login";
import Register from "./Register";

function App() {
  const [seats, setSeats] = useState([]);
  const [selectedSeats, setSelectedSeats] = useState([]);
  const [sectorId, setSectorId] = useState(1);
  const [isLogged, setIsLogged] = useState(!!localStorage.getItem("userId"));
  const [view, setView] = useState("login");
  const [myReservations, setMyReservations] = useState([]);
  const [showReservations, setShowReservations] = useState(false); // Para mostrar/ocultar la seccion

  const loadSeats = () => {
    fetch(`http://localhost:5171/api/v1/events/${sectorId}/seats`)
      .then(res => res.json())
      .then(data => setSeats(data))
      .catch(err => console.error(err));
  };

  useEffect(() => {
    if (!isLogged) return;
    loadSeats();
  }, [sectorId, isLogged]);

  const toggleSeat = (seatId, status) => {
    if (status !== 'Available') return;
    if (selectedSeats.includes(seatId)) {
      setSelectedSeats(selectedSeats.filter(id => id !== seatId));
    } else {
      setSelectedSeats([...selectedSeats, seatId]);
    }
  };

  const resetLocalSelection = () => {
    if (window.confirm("¿Deseas limpiar los asientos seleccionados actualmente?")) {
      setSelectedSeats([]);
    }
  };

  const handleConfirm = async () => {
    const userId = localStorage.getItem("userId");
    
    // VALIDACIÓN EXTRA: Si no hay usuario, no disparamos el fetch
    if (!userId) {
      alert("Error: No se encontró sesión de usuario. Por favor, reingresá.");
      return;
    }

    if (selectedSeats.length === 0) return;

    try {
      const response = await fetch("http://localhost:5171/api/v1/reservations", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ 
            userId: parseInt(userId), 
            seatIds: selectedSeats 
        })
      });

      if (!response.ok) {
        // Aquí capturamos los errores del ReservationResult (SeatNotFound, Conflict, etc.)
        const text = await response.text();
        alert("Error al reservar: " + text);
        return;
      }

      alert("Reserva realizada con éxito");
      setSelectedSeats([]); 
      loadSeats(); // Esto refresca el mapa y los pone en ROJO
    } catch (error) {
      console.error("Error de conexión:", error);
      alert("Error de conexión con el servidor");
    }
  };

const loadMyReservations = async () => {
    const userId = localStorage.getItem("userId");
    if (!userId) return;

    try {
      const response = await fetch(`http://localhost:5171/api/v1/reservations/user/${userId}`);
      if (response.ok) {
        const data = await response.json();
        setMyReservations(data);
        setShowReservations(true);
      } else {
        alert("No se pudieron cargar tus reservas.");
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("userId");
    setIsLogged(false);
    setView("login");
    setSelectedSeats([]); 
  };

  if (!isLogged) {
    if (view === "login") {
      return <Login onLogin={() => setIsLogged(true)} goToRegister={() => setView("register")} />;
    }
    return <Register goToLogin={() => setView("login")} />;
  }

  return (
    // CAMBIO: Fondo oscuro forzado y color de letra blanco
    <div style={{ 
      padding: '20px', 
      textAlign: 'center', 
      fontFamily: 'Arial, sans-serif',
      backgroundColor: '#1a1a1a', // Un gris casi negro muy pro
      color: '#ffffff',           // Letras blancas puras
      minHeight: '100vh'
    }}>
      
      {/* Título con color brillante */}
      <h1 style={{ color: '#ecf0f1', marginBottom: '30px' }}>Sistema de Ticketing - Entrega 1</h1>

      <button 
        onClick={handleLogout}
        style={{
          position: "absolute",
          top: "20px",
          right: "20px",
          padding: "8px 15px",
          backgroundColor: "#e74c3c",
          color: "white",
          border: "none",
          borderRadius: "5px",
          cursor: "pointer",
          fontWeight: 'bold'
        }}>
        Logout
      </button>

      {/* SECTORES: Botones con mejor contraste */}
      <div style={{ marginBottom: '20px', display: 'flex', gap: '15px', justifyContent: 'center' }}>
        <button 
          onClick={() => setSectorId(1)}
          style={{
            padding: '10px 20px',
            backgroundColor: sectorId === 1 ? '#3498db' : '#2c3e50',
            color: 'white',
            borderRadius: '5px',
            cursor: 'pointer',
            border: '1px solid #34495e',
            fontWeight: 'bold'
          }}>
          Platea Baja
        </button>

        <button 
          onClick={() => setSectorId(2)}
          style={{
            padding: '10px 20px',
            backgroundColor: sectorId === 2 ? '#3498db' : '#2c3e50',
            color: 'white',
            borderRadius: '5px',
            cursor: 'pointer',
            border: '1px solid #34495e',
            fontWeight: 'bold'
          }}>
          Platea Alta
        </button>
      </div>

      <h3 style={{ color: '#bdc3c7' }}>{sectorId === 1 ? 'Mapa: Platea Baja' : 'Mapa: Platea Alta'}</h3>

      <div style={{ marginBottom: '10px', fontSize: '1.1rem' }}>
        Asientos seleccionados: <strong style={{ color: '#3498db' }}>{selectedSeats.length}</strong>
      </div>

      {/* GRILLA: Fondo un poco más claro que el principal para que resalte */}
      <div style={{ 
        display: 'grid', 
        gridTemplateColumns: 'repeat(10, 45px)', 
        gap: '10px', 
        justifyContent: 'center',
        marginTop: '20px',
        backgroundColor: '#2c3e50',
        padding: '20px',
        borderRadius: '12px',
        boxShadow: '0 4px 15px rgba(0,0,0,0.3)'
      }}>
        {seats.map(seat => {
          const isSelected = selectedSeats.includes(seat.id);

          return (
            <div 
              key={seat.id}
              onClick={() => toggleSeat(seat.id, seat.status)}
              style={{
                width: '45px',
                height: '45px',
                backgroundColor: isSelected 
                  ? '#3498db' 
                  : (seat.status === 'Available' ? '#27ae60' : '#c0392b'),
                color: 'white',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                borderRadius: '6px',
                fontSize: '12px',
                fontWeight: 'bold',
                cursor: seat.status === 'Available' ? 'pointer' : 'not-allowed',
                transition: '0.2s',
                transform: isSelected ? 'scale(1.1)' : 'scale(1)',
                border: '1px solid rgba(255,255,255,0.1)'
              }}>
              {seat.seatNumber}
            </div>
          );
        })}
      </div>

     {/* BOTONES DE ACCIÓN */}
      <div style={{ marginTop: '30px', display: 'flex', justifyContent: 'center', gap: '15px' }}>
        <button 
          disabled={selectedSeats.length === 0}
          onClick={resetLocalSelection}
          style={{
            padding: '12px 25px',
            fontSize: '16px',
            backgroundColor: selectedSeats.length > 0 ? '#e74c3c' : '#7f8c8d',
            color: 'white',
            border: 'none',
            borderRadius: '5px',
            cursor: selectedSeats.length > 0 ? 'pointer' : 'not-allowed',
            fontWeight: 'bold',
            opacity: selectedSeats.length > 0 ? 1 : 0.5
          }}
        >
          Limpiar Selección
        </button>

        <button 
          disabled={selectedSeats.length === 0}
          onClick={handleConfirm}
          style={{
            padding: '12px 30px',
            fontSize: '16px',
            backgroundColor: selectedSeats.length > 0 ? '#2ecc71' : '#7f8c8d',
            color: 'white',
            border: 'none',
            borderRadius: '5px',
            cursor: selectedSeats.length > 0 ? 'pointer' : 'not-allowed',
            fontWeight: 'bold',
            opacity: selectedSeats.length > 0 ? 1 : 0.5
          }}
        >
          Confirmar Reserva ({selectedSeats.length})
        </button>
      </div>

      <div style={{ marginTop: '30px' }}>
        <button 
          onClick={showReservations ? () => setShowReservations(false) : loadMyReservations}
          style={{
            padding: '10px 20px',
            backgroundColor: 'transparent',
            color: '#00a8ff',
            border: '1px solid #00a8ff',
            borderRadius: '5px',
            cursor: 'pointer',
            fontWeight: 'bold'
          }}
        >
          {showReservations ? "Ocultar mis reservas" : "Ver mis reservas"}
        </button>
      </div>

      {showReservations && (
        <div style={{ 
          marginTop: '20px', 
          padding: '20px', 
          backgroundColor: '#1a1a1a', 
          borderRadius: '10px',
          border: '1px solid #333'
        }}>
          <h2 style={{ color: '#ffffff' }}>Mis Tickets</h2>
          {myReservations.length === 0 ? (
            <p style={{ color: '#888' }}>Aún no tenés asientos reservados.</p>
          ) : (
            <table style={{ width: '100%', borderCollapse: 'collapse', color: '#fff' }}>
              <thead>
                <tr style={{ borderBottom: '1px solid #444' }}>
                  <th style={{ padding: '10px' }}>Evento</th>
                  <th style={{ padding: '10px' }}>Asiento</th>
                  <th style={{ padding: '10px' }}>Fecha</th>
                </tr>
              </thead>
              <tbody>
                {myReservations.map((res, index) => (
    <tr key={index} style={{ borderBottom: '1px solid #222' }}>
      
      <td style={{ padding: '10px' }}>Concierto de Rock</td>
      
   <td style={{ padding: '10px' }}>
  
  Asiento N° {res.seatNumber || res.SeatNumber}
</td>
      
      <td style={{ padding: '10px' }}>
        {new Date(res.reservedAt).toLocaleDateString()}
      </td>
    </tr>
  ))}
              </tbody>
            </table>
          )}
        </div>
      )}

    </div> 
  );
}

export default App;

   