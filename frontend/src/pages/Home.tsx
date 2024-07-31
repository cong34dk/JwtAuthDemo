import React from "react";

const Home = (props: { name: string }) => {
    return (
        <div>
            {props.name ? `Chào ${props.name}` : 'Bạn chưa đăng nhập'}
        </div>
    );
};

export default Home;
