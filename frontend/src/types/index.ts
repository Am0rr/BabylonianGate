export type WeaponStatus = 'InStorage' | 'Deployed' | 'Maintenance' | 'Missing';
export type WeaponType = 'Rifle' | 'Pistol' | 'Sniper' | 'Launcher';

export interface Weapon {
    id: string;
    codename: string;
    serialNumber: string;
    caliber: string;
    type: WeaponType;
    condition: number;
    status: WeaponStatus;
    issuedToSoldierId?: string | null;
}