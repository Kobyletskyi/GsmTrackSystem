import cookie from 'react-cookie';

const id = cookie.load('userId');
console.log(id);
const initialState = {
    user: {
        Id:id,
        UserName:''
    },
    devices:[],
    track:{
        devices:[],
        coordinates:[]
    }
};

export default initialState;