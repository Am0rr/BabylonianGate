import {useState} from 'react';
import {WeaponCard} from './components/WeaponCard';
import type {Weapon} from './types';


function App() {
    const [weapons] = useState<Weapon[]>([
    {
      id: "guid-1", 
        codename: "BLACK WIDOW", 
        serialNumber: "SN-8842-X", 
        caliber: ".338 Lapua", 
        type: "Sniper", 
        condition: 98.5, 
        status: "InStorage"
    },
    { 
        id: "guid-2", 
        codename: "THUNDERBOLT", 
        serialNumber: "RPG-7V2", 
        caliber: "40mm", 
        type: "Launcher", // Был "Plasma", стал Launcher
        condition: 45.0, 
        status: "Maintenance" 
      },
      { 
        id: "guid-3", 
        codename: "M4 COMMANDO", 
        serialNumber: "AR-15-223", 
        caliber: "5.56 NATO", 
        type: "Rifle", 
        condition: 100.0, 
        status: "Deployed",
        issuedToSoldierId: "soldier-1"
      },
      { 
        id: "guid-4", 
        codename: "DESERT EAGLE", 
        serialNumber: "XX-50-AE", 
        caliber: ".50 AE", 
        type: "Pistol", 
        condition: 0.0, 
        status: "Missing" 
      },
    ]);

    return (
      <div className="min-h-screen bg-military-black text-military-green font-mono p-10 flex flex-col items-center justify-center">
        <div className = "max-w-6xl mx-auto">
            <header className = "flex flex-col md:flex-row justify-between items-end border-b-2 border-military-green/30 pb-6 mb-10 gap-4">
                <div>
                  <h1 className = "text-4xl md:text-5xl font-bold tracking-widest uppercase mb-2 text-white drop-shadow-[0_0_10px_rgba(255,255,255,0.3)]">
                    Babylonian<span className = "text-military-green">Gate</span>
                  </h1>
                  <p className = "text-xs text-gray-500 tracking-[0.3em]">WEAPON WAREHOUSE MANAGEMENT SYSTEM // V.1.0</p>
                </div>

                <div className = "text-right flex flex-col items-end">
                    <div className = "bg-military-dark/50 border border-military-green/30 px-4 py-2 rounded mb-2">
                      <span className = "text-military-red animate-pulse font-bold">● LIVE FEED</span>
                    </div>
                    <div className = "text-xs text-gray-600">
                        UNITS ONLINE: {weapons.filter(w => w.status !== 'Missing').length} / {weapons.length}
                    </div>
                </div>
            </header>

            <div className = "grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {weapons.map(weapon => (
                  <WeaponCard key = {weapon.id} weapon={weapon} />
                ))}
            </div>
        </div>
      </div>
    )
  }

  export default App;