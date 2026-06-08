import React from 'react';

const UserReservations = ({ reservations, onClose }) => {
  return (
    <div style={{ 
      backgroundColor: '#2c3e50', 
      padding: '20px', 
      borderRadius: '12px', 
      marginBottom: '20px',
      border: '1px solid #34495e',
      boxShadow: '0 4px 15px rgba(0,0,0,0.5)'
    }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <h2 style={{ margin: 0, color: '#f1c40f' }}>🎫 Mis Tickets</h2>
        <button onClick={onClose} style={{ background: 'none', border: 'none', color: '#bdc3c7', cursor: 'pointer', fontSize: '20px' }}>✖</button>
      </div>

      {reservations.length === 0 ? (
        <p>Aún no tienes reservas realizadas.</p>
      ) : (
        <div style={{ display: 'grid', gap: '10px' }}>
          {reservations.map((res) => (
            <div key={res.id} style={{ 
              backgroundColor: '#34495e', 
              padding: '15px', 
              borderRadius: '8px', 
              borderLeft: '5px solid #2ecc71',
              textAlign: 'left'
            }}>
              <h4 style={{ margin: '0 0 5px 0', color: '#fff' }}>{res.eventName}</h4>
              <p style={{ margin: 0, fontSize: '0.9rem' }}>
                Sector: <strong>{res.sectorName}</strong> | Asiento: <strong>{res.seatNumber}</strong>
              </p>
              <small style={{ color: '#95a5a6' }}>
                Fecha: {new Date(res.reservedAt).toLocaleString()}
              </small>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default UserReservations;