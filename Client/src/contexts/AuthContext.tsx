import { createContext, useContext, useEffect, useState } from "react";
import axios from "axios";

interface User {
  id: string;
  username: string;
  email: string;
  roles: string[];
}

interface AuthContextType {
  user: User | null;
  loading: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
  hasRole: (role: string) => boolean;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used within an AuthProvider");
  return context;
};

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  const fetchUser = async () => {
    try {
      const response = await axios.get("https://localhost:7018/api/User/me", {
        withCredentials: true,
      });

      const data = response.data.data;
      const formattedUser: User = {
        id: data.id,
        username: data.userName,
        email: data.email,
        roles: Array.isArray(data.roles) ? data.roles : [],
      };
      setUser(formattedUser);
    } catch {
      setUser(null);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUser();
  }, []);

  const login = async (email: string, password: string) => {
    await axios.post(
      "https://localhost:7018/api/Auth/login",
      { email, password },
      { withCredentials: true }
    );

    await fetchUser();
  };

  const logout = async () => {
    await axios.post(
      "https://localhost:7018/api/Auth/logout",
      {},
      { withCredentials: true }
    );
    setUser(null);
  };

  const hasRole = (role: string) => {
    return user?.roles?.includes(role) ?? false;
  };

  return (
    <AuthContext.Provider value={{ user, loading, login, logout, hasRole }}>
      {children}
    </AuthContext.Provider>
  );
};
