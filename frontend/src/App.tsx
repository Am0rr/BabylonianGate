import { useState, useEffect } from 'react';
import { Sidebar } from './components/Sidebar';
import { Header } from './components/Header';
import { StatsBanner } from './components/StatsBanner';
import { SearchBar } from './components/SearchBar';
import type { DashboardStats } from './types/types';

const initialStats: DashboardStats = {
  storage: { total: 0, readyPercentage: 0 },
  operational: { deployed: 0, repair: 0, missing: 0 },
  ammo: { totalRounds: 0, crates: 0 },
  personnel: { armed: 0, totalStaff: 0 }
};

function App() {
  const [stats, setStats] = useState<DashboardStats>(initialStats);
  const [isLoading, setIsLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    const timer = setTimeout(() => {
      setStats({
        storage: { total: 154, readyPercentage: 84 },
        operational: { deployed: 70, repair: 2, missing: 3 },
        ammo: { totalRounds: 42300, crates: 15 },
        personnel: { armed: 34, totalStaff: 120 }
      });
      setIsLoading(false);
    }, 1000); 
    return () => clearTimeout(timer);
  }, []);

  const handleSearch = (query: string) => {
    setSearchQuery(query);
    console.log('Searching:', query);
  };

  return (
    <div className="flex min-h-screen bg-[#050505] text-white font-mono">
      
      <Sidebar />
      
      <div className="flex-1 flex flex-col relative overflow-hidden">
        <Header />
        
        <main className="flex-1 overflow-y-auto p-8">
            <StatsBanner 
              stats={stats} 
              isLoading={isLoading} 
              onFilterClick={(filter) => console.log('Filter:', filter)}
            />
            
            <SearchBar 
              value={searchQuery} 
              onChange={handleSearch} 
            />

            <div className="mt-8 border border-dashed border-white/10 rounded h-96 flex items-center justify-center text-gray-600 uppercase tracking-widest text-sm">
                Inventory Table Area <br/>
                (Filter: {searchQuery || "None"})
            </div>
        </main>
      </div>
    </div>
  )
}

export default App;