import type {Weapon, WeaponStatus, WeaponType} from '../types';
import {
    Crosshair, Hash, Activity, ShieldAlert, Package, Wrench,
    Target, Bomb, Scan, type LucideIcon
} from 'lucide-react';

interface Props {
    weapon: Weapon;
}

const TYPE_ICONS: Record<WeaponType, LucideIcon> = {
    Sniper: Target,
    Pistol: Scan,
    Launcher: Bomb,
    Rifle: Crosshair
}

const getStatusConfig = (status: WeaponStatus) => {
    switch (status) {
        case 'Deployed': return {color: 'text-yellow-500', bg: 'bg-yellow-500', icon: Activity, label: 'DEPLOYED'};
        case 'InStorage': return {color: 'text-military-green', bg: 'bg-military-green', icon: Package, label: 'IN STORAGE'};
        case 'Maintenance': return {color: 'text-orange-500', bg: 'bg-orange-500', icon: Wrench, label: 'REPAIR'};
        case 'Missing': return {color: 'text-red-600', bg: 'bg-red-600', icon: ShieldAlert, label: 'MISSING'};
        default: return {color: 'text-gray-500', bg: 'bg-gray-500', icon: Hash, label: 'UNKNOWN'};
    }
};

export const WeaponCard = ({weapon}: Props) => {
    const statusConfig = getStatusConfig(weapon.status);
    const StatusIcon = statusConfig.icon;
    const TypeIcon = TYPE_ICONS[weapon.type];

    return (
        <div className = "relative border border-military-green/30 bg-military-dark/80 p-5 rounded hover:border-military-green hover:shadow-[0_0_15px_rgba(16,185,129,0.2)] transition-all duration-300 group">

            <div className = "flex justify-between items-start mb-4">
                <div>
                    <h3 className = "text-xl font-bold text-white tracking-wider group-hover:text-military-green transition-colors">
                        {weapon.codename}
                    </h3>
                    <div className = "flex items-center gap-1 text-xs text-gray-500 font-mono mt-1">
                        <Hash size={10} /> {weapon.serialNumber}
                    </div>
                </div>

                <div className = {`flex items-center gap-1 px-2 py-1 rounded border border-white/10 ${statusConfig.color} bg-white/5 text-[10px] font-bold tracking-wider`}>
                    <StatusIcon size={10} />
                    {statusConfig.label}
                </div>
            </div>

            <div className = "space-y-3 font-mono text-sm relative z-10">
                <div className = "flex justify-between text-gray-400 text-xs uppercase items-center">
                    <span>CAl: <span className = "text-white">{weapon.caliber}</span></span>
                    
                    <span className = "flex items-center gap-1 border border-gray-700 px-2 py-0.5 rounded bg-black/40">
                        <TypeIcon size={10} className = "text-military-green"/>
                        <span className = "text-gray-300">{weapon.type}</span>
                    </span>
                </div>

                <div>
                    <div className = "flex justify-between text-xs mb-1">
                        <span className = "text-gray-500">INTEGRITY</span>
                        <span className ={weapon.condition < 30 ? 'text-red-500' : 'text-military-green'}>
                            {weapon.condition}%
                        </span>
                    </div>
                    <div className = "h-1.5 w-full bg-black rounded overflow-hidden border border-white/10">
                        <div 
                            className = {`h-full transition-all duration-500 ${weapon.condition < 30 ? 'bg-red-600' : 'bg-military-green'}`}
                            style = {{width: `${weapon.condition}%`}}
                        />
                    </div>
                </div>

                <div className = "pt-2 flex justify-end">
                    <button className = "opacity-0 group-hover:opacity-100 transition-opacity flex items-center gap-1 text-xs border border-military-green text-military-green px-3 py-1 hover:bg-military-green hover:text-black">
                        MANAGE <Crosshair size={12} />
                    </button>
                </div>
            </div>
        </div>
    );
};