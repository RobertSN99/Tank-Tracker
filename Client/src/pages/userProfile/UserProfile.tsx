import { useParams } from "react-router-dom";
import "./UserProfile.css";
import { useEffect, useState } from "react";
import axios from "axios";

interface UserData {
  createdAt: string;
  email: string;
  id: string;
  roles: string[];
  updatedAt: string;
  userName: string;
}

function UserProfile() {
  const { userName } = useParams<{ userName: string }>();
  const [userData, setUserData] = useState<UserData>({
    createdAt: "",
    email: "",
    id: "",
    roles: [],
    updatedAt: "",
    userName: "",
  });

  const roleColor = (role: string) => {
    switch (role) {
      case "Administrator":
        return { color: "darkred" };
      case "Moderator":
        return { color: "orange" };
      case "User":
        return { color: "#1ab0ab" };
    }
  };
  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await axios.get(
          `https://localhost:7018/api/user/name/${userName}`
        );

        setUserData({
          createdAt: response.data.data.createdAt,
          email: response.data.data.email,
          id: response.data.data.id,
          roles: response.data.data.roles,
          updatedAt: response.data.data.updatedAt,
          userName: response.data.data.userName,
        });
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    };

    fetchUserData();
  }, [userName]);
  return (
    <div className="userprofile-container">
      <div className="userprofile-card">
        <div className="userprofile-header">
          <div className="userprofile-avatar">
            {userData.userName.charAt(0).toUpperCase()}
          </div>
          <div>
            <h2 className="userprofile-name">{userData.userName}</h2>
            <p className="userprofile-email">{userData.email}</p>
          </div>
        </div>

        <div className="userprofile-section">
          <div className="userprofile-roles">
            {userData.roles.map((role) => (
              <span
                key={role}
                className="userprofile-role"
                style={roleColor(role)}
              >
                {role}
              </span>
            ))}
          </div>
        </div>

        <div className="userprofile-section">
          <span className="userprofile-label">Member since:</span>
          {new Date(userData.createdAt).toLocaleDateString("en-GB", {
            day: "numeric",
            month: "long",
            year: "numeric",
          })}
        </div>
      </div>
    </div>
  );
}

export default UserProfile;
