export interface DashboardStats {
    storage: {
      total: number;
      readyPercentage: number;
    };
    operational: {
      deployed: number;
      repair: number;
      missing: number;
    };
    ammo: {
      totalRounds: number;
      crates: number;
    };
    personnel: {
      armed: number;
      totalStaff: number;
    };
}