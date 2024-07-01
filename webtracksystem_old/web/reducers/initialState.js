import cookie from 'react-cookie';

const id = cookie.load('userId');
console.log(id);
const initialState = {
    user: {
        id:id,
        userName:''
    },
    devices:[],
    track:{
        devices:[],
        coordinates:[]
    }
};

export default initialState;