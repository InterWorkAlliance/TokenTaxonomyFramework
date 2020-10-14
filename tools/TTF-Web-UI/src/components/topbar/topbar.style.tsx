import styled from "styled-components";
import { transition, borderRadius, boxShadow } from "../../settings/style-util";
import WithDirection from "../../settings/withDirection";

export const TopbarWrapper = styled.div`
    .topbar {
        display: flex;
        justify-content: space-between;
        background-color: #ffffff;
        border-bottom: 1px solid rgba(0, 0, 0, 0.1);
        padding: 0 31px 0 265px";
        z-index: 1000;
        ${transition()};
        @media only screen and (max-width: 767px) {
        padding: 0px 15px 0px 260px !important};
        }

    }
`;

export default TopbarWrapper;
